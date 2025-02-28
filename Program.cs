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

                if (!Db.CheckIfDbExists())
                {
                    string masterPassword = EncryptUtils.CreateMasterPassword();
                    byte[] key = EncryptUtils.DeriveKeyFromMasterPassword(masterPassword);

                    Store.KeyManager.SetKey(key);
                    Db.CreateConnection(key, true);
                }
                else { }
            }
        }
    }
}
