using System;
using System.Collections.Generic;
using System.Linq;
using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic
{
  public class SnackMachine : AggregateRoot
  {
    public virtual Money MoneyInside { get; protected set; }
    public virtual decimal MoneyInTransaction { get; protected set; }
    protected virtual IList<Slot> Slots { get; set; }

    public SnackMachine()
    {
      MoneyInside = None;
      MoneyInTransaction = 0;
      Slots = new List<Slot>
      {
        new Slot(this, 1),
        new Slot(this, 2),
        new Slot(this, 3),
      };
    }

    public virtual void InsertMoney(Money money)
    {
      Money[] coinsAndNotes = {Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar};
      if (!coinsAndNotes.Contains(money))
        throw new InvalidOperationException();

      MoneyInTransaction += money.Amount;
      MoneyInside += money;
    }

    public virtual void ReturnMoney()
    {
      Money moneyToReturn = MoneyInside.Allocate(MoneyInTransaction);
      MoneyInside -= moneyToReturn;
      MoneyInTransaction = 0;
    }

    public virtual void BuySnack(int position)
    {
      var slot = GetSlotAt(position);
      if(slot.SnackPile.Price > MoneyInTransaction)
        throw new InvalidOperationException();

      slot.SnackPile = slot.SnackPile.SubtractOne();

      var change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
      if(change.Amount < MoneyInTransaction - slot.SnackPile.Price)
        throw new InvalidOperationException();

      MoneyInside -= change;
      MoneyInTransaction = 0;
    }

    public virtual void LoadSnacks(int position, SnackPile snackPile)
    {
      var slot = Slots.Single(x => x.Position.Equals(position));
      slot.SnackPile = snackPile;
    }

    public virtual SnackPile GetSnackPile(int position)
    {
      return GetSlotAt(position).SnackPile;
    }

    private Slot GetSlotAt(int position)
    {
      return Slots.Single(x => x.Position.Equals(position));
    }

    public virtual void LoadMoney(Money money)
    {
      MoneyInside += money;
    }
  }
}