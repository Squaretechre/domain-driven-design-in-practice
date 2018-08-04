using System;
using DddInPractice.Logic;
using FluentAssertions;
using Xunit;

namespace DddInPractice.Tests
{
  public class MoneySpecs
  {
    [Fact]
    public void sum_of_two_moneys_produces_correct_result()
    {
      var money1 = new Money(1, 2, 3, 4, 5, 6);
      var money2 = new Money(1, 2, 3, 4, 5, 6);

      var sum = money1 + money2;

      sum.OneCentCount.Should().Be(2);
      sum.TenCentCount.Should().Be(4);
      sum.QuarterCount.Should().Be(6);
      sum.OneDollarCount.Should().Be(8);
      sum.FiveDollarCount.Should().Be(10);
      sum.TwentyDollarCount.Should().Be(12);
    }

    [Fact]
    public void two_money_instances_are_equal_if_they_contain_same_amount()
    {
      var money1 = new Money(1, 2, 3, 4, 5, 6);
      var money2 = new Money(1, 2, 3, 4, 5, 6);

      money1.Should().Be(money2);
      money1.GetHashCode().Should().Be(money2.GetHashCode());
    }

    [Fact]
    public void two_money_instances_are_not_equal_if_they_contain_different_amounts()
    {
      var dollar = new Money(0, 0, 0, 1, 0, 0);
      var hundredCents = new Money(100, 0, 0, 0, 0, 0);

      dollar.Should().NotBe(hundredCents);
      dollar.GetHashCode().Should().NotBe(hundredCents.GetHashCode());
    }

    [Theory]
    [InlineData(-1, 0, 0, 0, 0, 0)]
    [InlineData(0, -2, 0, 0, 0, 0)]
    [InlineData(0, 0, -3, 0, 0, 0)]
    [InlineData(0, 0, 0, -4, 0, 0)]
    [InlineData(0, 0, 0, 0, -5, 0)]
    [InlineData(0, 0, 0, 0, 0, -6)]
    public void cannot_create_money_with_negative_value(
      int oneCountCount,
      int tenCentCount,
      int quarterCount,
      int oneDollarCount,
      int fiveDollarCount,
      int twentyDollarCount)
    {
      Action action = () => new Money(
        oneCountCount,
        tenCentCount,
        quarterCount,
        oneDollarCount,
        fiveDollarCount,
        twentyDollarCount);

      action.Should().Throw<InvalidOperationException>();
    }
  }
}