namespace DddInPractice.Logic.Utils
{
  public class Initer
  {
    public static void Init(string connectionString)
    {
      SessionFactory.Init(connectionString);
    }
  }
}