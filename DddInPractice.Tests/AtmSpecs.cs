using System.Linq;
using System.ServiceModel.Syndication;
using DddInPractice.Logic.Atms;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.SharedKernel;
using DddInPractice.Logic.Utils;
using static DddInPractice.Logic.SharedKernel.Money;
using FluentAssertions;
using Xunit;

namespace DddInPractice.Tests
{
  public class AtmSpecs
  {
    [Fact]
    public void take_money_exchanges_money_with_commission()
    {
      var atm = new Atm();
      atm.LoadMoney(Dollar);

      atm.TakeMoney(1m);

      atm.MoneyInside.Amount.Should().Be(0m);
      atm.MoneyCharged.Should().Be(1.01m);
    }

    [Fact]
    public void commission_is_at_least_one_cent()
    {
      var atm = new Atm();
      atm.LoadMoney(Cent);

      atm.TakeMoney(0.01m);

      atm.MoneyCharged.Should().Be(0.02m);
    }

    [Fact]
    public void commission_is_rounded_up_to_the_next_cent()
    {
      var atm = new Atm();
      atm.LoadMoney(Dollar + TenCent);

      atm.TakeMoney(1.1m);

      atm.MoneyCharged.Should().Be(1.12m);
    }

    [Fact]
    public void take_money_raises_an_event()
    {
      var atm = new Atm();
      atm.LoadMoney(Dollar);

      atm.TakeMoney(1m);

      atm.ShouldContainBalanceChangedEvent(1.01m);
    }
  }

  internal static class AtmExtensions
  {
    public static void ShouldContainBalanceChangedEvent(this Atm atm, decimal delta)
    {
      BalanceChangedEvent domainEvent = (BalanceChangedEvent) atm.DomainEvents
        .SingleOrDefault(x => x.GetType() == typeof(BalanceChangedEvent));

      domainEvent.Should().NotBeNull();
      domainEvent.Delta.Should().Be(delta);
    }
  }
}