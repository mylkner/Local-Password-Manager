namespace Utils
{
    public static class Confirm
    {
        public static bool ConfirmAction()
        {
            string? choice = null;

            Console.WriteLine("\nAre you sure? y/n");

            while (choice != "y" && choice != "n")
            {
                choice = Console.ReadLine()?.ToLower();
            }

            return choice == "y";
        }
    }
}
