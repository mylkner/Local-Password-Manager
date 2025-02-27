namespace Utils
{
    class Confirm
    {
        public static bool ConfirmAction()
        {
            string? choice;

            Console.WriteLine("\nAre you sure? y/n");

            do
            {
                choice = Console.ReadLine()?.ToLower();
            } while (choice != "y" && choice != "n");

            return choice == "y";
        }
    }
}
