﻿using System;
using System.Linq;
using DddInPractice.Logic;
using FluentAssertions;
using Xunit;

using static DddInPractice.Logic.Money;

namespace DddInPractice.Tests
{
  public class SnackMachineSpecs
  {
    [Fact]
    public void return_money_empties_money_in_transaction()
    {
      var snackMachine = new SnackMachine();
      snackMachine.InsertMoney(Dollar);

      snackMachine.ReturnMoney();

      snackMachine.MoneyInTransaction.Amount.Should().Be(0m);
    }

    [Fact]
    public void inserted_money_goes_to_money_in_transaction()
    {
      var snackMachine = new SnackMachine();

      snackMachine.InsertMoney(Cent);
      snackMachine.InsertMoney(Dollar);

      snackMachine.MoneyInTransaction.Amount.Should().Be(1.01m);
    }

    [Fact]
    public void cannot_insert_more_than_one_coin_or_note_at_a_time()
    {
      var snackMachine = new SnackMachine();
      var twoCent = Cent + Cent;

      Action action = () => snackMachine.InsertMoney(twoCent);

      action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void buy_snack_trades_inserted_money_for_snack()
    {
      var snackMachine = new SnackMachine();
      snackMachine.LoadSnacks(1, new SnackPile(new Snack("Some snack"), 10, 1m));
      snackMachine.InsertMoney(Dollar);

      snackMachine.BuySnack(1);

      snackMachine.MoneyInTransaction.Should().Be(None);
      snackMachine.MoneyInside.Amount.Should().Be(1m);
      snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
    }
  }
}