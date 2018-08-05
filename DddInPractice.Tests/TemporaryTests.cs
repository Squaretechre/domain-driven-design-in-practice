using System;
using DddInPractice.Logic;
using NHibernate;
using Xunit;

namespace DddInPractice.Tests
{
  public class TemporaryTests
  {
    //[Fact]
    public void test()
    {
      SessionFactory.Init(@"Server=.;Database=DddInPractice;Trusted_Connection=true");

      const long id = 1;

      using (ISession session = SessionFactory.OpenSession())
      {
        var snackMachine = session.Get<SnackMachine>(id);
      }
    }

    //[Fact]
    public void another_test()
    {
      SessionFactory.Init(@"Server=.;Database=DddInPractice;Trusted_Connection=true");

      using (ISession session = SessionFactory.OpenSession())
      {
        var repository = new SnackMachineRepository();
        var snackMachine = repository.GetById(1);
        snackMachine.InsertMoney(Money.Dollar);
        snackMachine.InsertMoney(Money.Dollar);
        snackMachine.InsertMoney(Money.Dollar);
        snackMachine.BuySnack(1);
        repository.Save(snackMachine);
      }
    }
  }
}