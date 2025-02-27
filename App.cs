using Encryption;
using SQLite;

namespace PasswordManager
{
    public static class App
    {
        public static void Run()
        {
            Console.WriteLine("Welcome to your password manager.");

            if (!Db.CheckForDummyEntry())
            {
                Db.SetDummyEntry();
            }
            else { }
        }
    }
}
