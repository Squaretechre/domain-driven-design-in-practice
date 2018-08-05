using DddInPractice.Logic;
using NHibernate;

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
  }
}