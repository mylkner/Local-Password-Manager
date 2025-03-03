namespace Utils
{
    public static class Commands
    {
        public static void Help() { }

        public static void List() { }

        public static void Add() { }

        public static void Get() { }

        public static void Del() { }

        public static void Exit()
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}
