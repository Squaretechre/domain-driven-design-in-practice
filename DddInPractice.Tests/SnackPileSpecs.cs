using System;
using DddInPractice.Logic;
using DddInPractice.Logic.SnackMachines;
using FluentAssertions;
using Xunit;

using static DddInPractice.Logic.SnackMachines.Snack;

namespace DddInPractice.Tests
{
  public class SnackPileSpecs
  {
    [Fact]
    public void cannot_create_with_negative_quantity()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(Chocolate, -1, 0);
      };

      action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void cannot_create_with_negative_price()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(Chocolate, 0, -1);
      };

      action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void throw_when_price_more_precise_than_one_cent()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(Chocolate, 0, 1.123m);
      };

      action.Should().Throw<InvalidOperationException>();
    }
  }
}