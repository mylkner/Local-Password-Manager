using System.Data.Entity.Infrastructure;

namespace Verification
{
    class MasterPassword()
    {
        public static bool CheckIfExists()
        {
            return false;
        }

        public static void Create()
        {
            string? masterPassword;
            bool equal;

            do
            {
                Console.WriteLine(
                    "Please enter a master password. This password will be used for encryption and CANNOT be changed."
                );
                masterPassword = Console.ReadLine();

                Console.WriteLine("\nPlease enter your password again to confirm.");
                string? reEntry = Console.ReadLine();

                equal = masterPassword == reEntry;

                if (!equal)
                    Console.WriteLine("\nPasswords do not match.");
            } while (!equal);

            if (masterPassword is not null)
                SQLite.Db.SetMasterPassword(masterPassword);
        }

        public static bool VerifyLogin()
        {
            Console.WriteLine("Please Provide master password.");
            return true;
        }
    }
}
