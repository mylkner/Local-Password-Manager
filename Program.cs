using Encryption;
using SQLiteDB;
using Utils;

namespace PasswordManager
{
    public static class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to your password manager.");

            if (!Db.DbExists())
            {
                bool equal = false;

                while (!equal)
                {
                    Console.WriteLine(
                        "Please enter a master password. This password will be used for encryption and CANNOT be changed."
                    );

                    string? masterPassword =
                        Console.ReadLine()
                        ?? throw new ArgumentNullException("Master password cannot be null");

                    Console.WriteLine("\nPlease enter your password again to confirm.");
                    string? reEntry = Console.ReadLine();

                    equal = masterPassword == reEntry;

                    if (!equal)
                    {
                        Console.WriteLine("\nPasswords do not match.");
                    }
                    else
                    {
                        Console.WriteLine("\nPassword set.");
                        Console.WriteLine("Attempting database creation and connection...");
                        byte[]? salt = Db.OpenDb(masterPassword); //salt will be null here, OpenDb uses GetKeySalt to verify password for subsuquent logins
                        Initialise(masterPassword, salt);
                        break;
                    }
                }
            }
            else
            {
                byte[]? salt = null;

                while (salt == null)
                {
                    Console.WriteLine("Please input your master password.");

                    string masterPassword =
                        Console.ReadLine()
                        ?? throw new ArgumentNullException("Input cannot be null.");

                    Console.WriteLine("\nAttempting to connect to database...");

                    salt = Db.OpenDb(masterPassword);

                    if (salt == null)
                    {
                        Console.WriteLine("Failed to connect.");
                    }
                    else
                    {
                        Initialise(masterPassword, salt);
                        break;
                    }
                }
            }

            while (true)
            {
                Console.WriteLine("\nType `help` to see list of commands.");
                string? option =
                    Console.ReadLine() ?? throw new ArgumentNullException("Input cannot be null.");
                string[] commandParts = option.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                switch (commandParts[0].ToLower())
                {
                    case "help":
                        Commands.Help();
                        break;
                    case "list":
                        Commands.List();
                        break;
                    case "get":
                        if (commandParts.Length > 1)
                        {
                            Commands.Get(commandParts[1]);
                        }
                        else
                        {
                            Console.WriteLine("\nPlease provide the title you want to get.");
                            string? title =
                                Console.ReadLine()
                                ?? throw new ArgumentNullException("Title cannot be null.");
                            Commands.Get(title);
                        }
                        break;
                    case "add":
                        Commands.Add();
                        break;
                    case "delete":
                        if (commandParts.Length > 1)
                        {
                            Commands.Delete(commandParts[1]);
                        }
                        else
                        {
                            Console.WriteLine("\nPlease provide the title you want to delete.");
                            string? title =
                                Console.ReadLine()
                                ?? throw new ArgumentNullException("Title cannot be null.");
                            Commands.Delete(title);
                        }
                        break;
                    case "exit":
                        Commands.Exit();
                        break;
                    default:
                        Console.WriteLine("Command not recognised.");
                        break;
                }
            }
        }

        static void Initialise(string masterPassword, byte[]? salt)
        {
            Console.WriteLine("Deriving key from master password...");
            EncryptUtils.DeriveKeyFromMasterPassword(masterPassword, salt);
            Console.WriteLine("Connected.");
        }
    }
}
