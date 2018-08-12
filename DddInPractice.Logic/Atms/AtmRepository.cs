using System.Collections.Generic;
using System.Linq;
using DddInPractice.Logic.Common;
using DddInPractice.Logic.Utils;
using NHibernate;

namespace DddInPractice.Logic.Atms
{
  public class AtmRepository : Repository<Atm>
  {
    public IReadOnlyList<AtmDto> GetAtmList()
    {
      using (ISession session = SessionFactory.OpenSession())
      {
        return session.Query<Atm>()
          .ToList()
          .Select(x => new AtmDto(x.Id, x.MoneyInside.Amount))
          .ToList();
      }
    }
  }
}