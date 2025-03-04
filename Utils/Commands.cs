using Encryption;
using SQLiteDB;

namespace Utils
{
    public static class Commands
    {
        public static void Help()
        {
            Console.WriteLine("\nCommands are not case sensitive.");
            Console.WriteLine("List - lists all entries.");
            Console.WriteLine("Add - add an entry.");
            Console.WriteLine("Get x - retrieves a password for x, where x is the title.");
            Console.WriteLine("Delete x - delete an entry where x is the title of the entry.");
            Console.WriteLine("Exit - exit the programme.");
        }

        public static void List()
        {
            Console.WriteLine("\nRetrieving entries...");
            Db.GetEntries();
        }

        public static void Get()
        {
            Console.WriteLine("a");
        }

        public static void Add()
        {
            Console.WriteLine("\nInput a title to add:");
            string? title =
                Console.ReadLine() ?? throw new ArgumentNullException("Title cannot be null.");

            Console.WriteLine("\nEnter a password:");
            string? pwd =
                Console.ReadLine() ?? throw new ArgumentNullException("Password cannot be null.");

            Console.WriteLine("\nEncrypting password...");
            (byte[] encryptedPassword, byte[] iv) = EncryptUtils.EncryptPassword(pwd);
            Console.WriteLine("Success!");
            Console.WriteLine("\nAdding to database...");
            Db.AddEntry(title, iv, encryptedPassword);
            Console.WriteLine("Success!");
        }

        public static void Delete()
        {
            Console.WriteLine("a");
        }

        public static void Exit()
        {
            Store.KeyManager.VoidKey();
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}
