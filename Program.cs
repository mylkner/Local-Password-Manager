using System.Reflection;
using Encryption;
using SQLite;

namespace PasswordManager
{
    public static class Program
    {
        static void Main()
        {
            {
                Console.WriteLine("Welcome to your password manager.");
                string? masterPassword;

                if (!Db.CheckIfDbExists())
                {
                    masterPassword = EncryptUtils.CreateMasterPassword();
                    Console.WriteLine("\nAttempting database creation and connection...");
                }
                else
                {
                    Console.WriteLine("Please input your master password.");
                    masterPassword =
                        Console.ReadLine()
                        ?? throw new ArgumentNullException("Input cannot be null.");
                    Console.WriteLine("\nAttempting to connect to database...");
                }

                Initialise(masterPassword);
            }
        }

        private static void Initialise(string masterPassword)
        {
            Db.CreateConnection(masterPassword);
            Console.WriteLine("Succesfully connected to database.");
            Console.WriteLine("\nDeriving key from master password...");
            EncryptUtils.DeriveKeyFromMasterPassword(masterPassword);
            Console.WriteLine("Key derived.");
        }
    }
}
