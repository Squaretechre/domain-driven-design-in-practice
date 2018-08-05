using System;
using DddInPractice.Logic;
using FluentAssertions;
using Xunit;

namespace DddInPractice.Tests
{
  public class SnackPileSpecs
  {
    [Fact]
    public void cannot_create_with_negative_quantity()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(new Snack("a snack"), -1, 0);
      };

      action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void cannot_create_with_negative_price()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(new Snack("a snack"), 0, -1);
      };

      action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void throw_when_price_more_precise_than_one_cent()
    {
      Action action = () =>
      {
        var snackPile = new SnackPile(new Snack("a snack"), 0, 1.123m);
      };

      action.Should().Throw<InvalidOperationException>();
    }
  }
}