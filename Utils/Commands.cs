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
            Console.WriteLine("a");
        }

        public static void Add()
        {
            Console.WriteLine("a");
        }

        public static void Get()
        {
            Console.WriteLine("a");
        }

        public static void Delete()
        {
            Console.WriteLine("a");
        }

        public static void Exit()
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}
