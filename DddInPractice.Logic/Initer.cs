namespace DddInPractice.Logic
{
  public class Initer
  {
    public static void Init(string connectionString)
    {
      SessionFactory.Init(connectionString);
    }
  }
}