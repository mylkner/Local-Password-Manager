using System.Data.SQLite;
using Encryption;

namespace SQLite
{
    public static class Db
    {
        private static SQLiteConnection? ConnectToDb(byte[] key)
        {
            return default;
        }

        public static bool CheckForDummyEntry()
        {
            return false;
        }

        public static void SetDummyEntry()
        {
            string masterPassword = EncryptUtils.CreateMasterPassword();
            (byte[] salt, byte[] key) = EncryptUtils.DeriveKeyFromMasterPassword(masterPassword);
            (byte[] encryptedPassword, byte[] iv) = EncryptUtils.EncryptPassword(
                "DummyEntry123",
                key
            );

            Store.KeyManager.SetKey(key);

            Console.WriteLine(
                $"ED: {Convert.ToBase64String(encryptedPassword)}\nIV: {Convert.ToBase64String(iv)}\nSalt: {Convert.ToBase64String(salt)}"
            );
            Console.WriteLine("Password Saved");
        }

        private static bool AddEntry()
        {
            return default;
        }

        private static bool DeleteEntry()
        {
            return default;
        }
    }
}
