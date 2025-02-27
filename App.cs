namespace PasswordManager
{
    class App
    {
        private string _key = "";

        public static void Run()
        {
            Console.WriteLine("Welcome to your password manager.");

            if (!Verification.MasterPassword.CheckIfExists())
            {
                Verification.MasterPassword.Create();
            }
            else
            {
                Verification.MasterPassword.VerifyLogin();
            }
        }
    }
}
