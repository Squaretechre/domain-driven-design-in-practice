using System;
using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;

namespace DddInPractice.Logic.SnackMachines
{
  public class SnackMachine : AggregateRoot
  {
    public virtual Money MoneyInside { get; protected set; }
    public virtual decimal MoneyInTransaction { get; protected set; }
    protected virtual IList<Slot> Slots { get; set; }

    public SnackMachine()
    {
      MoneyInside = Money.None;
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
      Money[] coinsAndNotes = {Money.Cent, Money.TenCent, Money.Quarter, Money.Dollar, Money.FiveDollar, Money.TwentyDollar};
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

    public virtual string CanBuySnack(int position)
    {
      var snackPile = GetSnackPile(position);

      if (snackPile.Quantity == 0)
        return "The snack pile is empty";

      if (MoneyInTransaction < snackPile.Price)
        return "Not enough money";

      if (!MoneyInside.CanAllocate(MoneyInTransaction - snackPile.Price))
        return "Not enough change";

      return string.Empty;
    }

    public virtual void BuySnack(int position)
    {
      if(CanBuySnack(position) != string.Empty)
        throw new InvalidOperationException();

      var slot = GetSlotAt(position);
      slot.SnackPile = slot.SnackPile.SubtractOne();

      var change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
      MoneyInside -= change;
      MoneyInTransaction = 0;
    }

    public virtual void LoadSnacks(int position, SnackPile snackPile)
    {
      var slot = Slots.Single(x => x.Position.Equals(position));
      slot.SnackPile = snackPile;
    }

    public virtual IReadOnlyList<SnackPile> GetAllSnackPiles()
    {
      return Slots
        .OrderBy(x => x.Position)
        .Select(x => x.SnackPile)
        .ToList();
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

    public virtual Money UnloadMoney()
    {
      if (MoneyInTransaction > 0)
        throw new InvalidOperationException();

      Money money = MoneyInside;
      MoneyInside = Money.None;
      return money;
    }
  }
}