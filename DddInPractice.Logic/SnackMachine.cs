using System;
using System.Collections.Generic;
using System.Linq;
using static DddInPractice.Logic.Money;

namespace DddInPractice.Logic
{
  public class SnackMachine : AggregateRoot
  {
    public virtual Money MoneyInside { get; protected set; }
    public virtual Money MoneyInTransaction { get; protected set; }
    protected virtual IList<Slot> Slots { get; set; }

    public SnackMachine()
    {
      MoneyInside = None;
      MoneyInTransaction = None;
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

      MoneyInTransaction += money;
    }

    public virtual void ReturnMoney()
    {
      MoneyInTransaction = None;
    }

    public virtual void BuySnack(int position)
    {
      var slot = GetSlotAt(position);
      slot.SnackPile = slot.SnackPile.SubtractOne();

      MoneyInside += MoneyInTransaction;
      MoneyInTransaction = None;
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
  }
}