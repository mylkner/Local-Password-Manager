using Encryption;
using SQLiteDB;
using TextCopy;

namespace Utils
{
    public static class Commands
    {
        public static void Help()
        {
            Console.WriteLine("\nCommands are not case sensitive.");
            Console.WriteLine("List - lists all entries.");
            Console.WriteLine("Get <title> - retrieves the password for <title>.");
            Console.WriteLine("Add - add an entry.");
            Console.WriteLine("Delete <title> - delete <title> from database.");
            Console.WriteLine("Exit - exit the programme.");
        }

        public static void List()
        {
            Console.WriteLine("\nRetrieving entries...");
            Db.ListEntries();
        }

        public static async void Get(string title)
        {
            Console.WriteLine($"\nGetting password for {title}...");
            var result = Db.GetEntry(title);

            if (result == null)
            {
                Console.WriteLine("No entry found for given title.");
                return;
            }

            byte[] iv = result.Value.iv;
            byte[] encryptedPassword = result.Value.encryptedPassword;
            Console.WriteLine("Decrypting password...");
            string decryptedPassword = EncryptUtils.DecryptPassword(iv, encryptedPassword);

            ClipboardService.SetText(decryptedPassword);
            Console.WriteLine(
                $"Password copied to clipboard. Make sure to clear clipboard history after use. (if enabled)"
            );
        }

        public static void Add()
        {
            Console.WriteLine("\nInput a title to add:");
            string? title =
                Console.ReadLine() ?? throw new ArgumentNullException("Title cannot be null.");

            Console.WriteLine("\nSelect password creation method (input either 1 or 2):");
            Console.WriteLine("1. Manual.");
            Console.WriteLine("2. Automatic (generates password for you).");
            string? choice = null;

            while (choice != "1" && choice != "2")
            {
                choice = Console.ReadLine();
            }

            string? pwd;

            if (choice == "1")
            {
                pwd =
                    Console.ReadLine()
                    ?? throw new ArgumentNullException("Password cannot be null.");
            }
            else
            {
                pwd = EncryptUtils.RandomPasswordGenerator();
            }

            Console.WriteLine("\nEncrypting password...");
            (byte[] encryptedPassword, byte[] iv) = EncryptUtils.EncryptPassword(pwd);
            Console.WriteLine("Adding to database...");
            Db.AddEntry(title, iv, encryptedPassword);
        }

        public static void Delete(string title)
        {
            Console.WriteLine($"\nDeleting {title}...");
            Db.DeleteEntry(title);
        }

        public static void Exit()
        {
            Store.KeyManager.VoidKey();
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}
