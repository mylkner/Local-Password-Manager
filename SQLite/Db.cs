using System.Data.SQLite;

namespace SQLite
{
    class Db
    {
        public static void SetMasterPassword(string masterPassword)
        {
            string dummy = "DummyPassword123";
            (byte[] salt, byte[] key) = Encryption.EncryptUtils.DeriveKeyFromMasterPassword(
                masterPassword
            );
            (byte[] encryptedData, byte[] iv) = Encryption.EncryptUtils.EncryptPassword(dummy, key);

            Console.WriteLine(
                $"ED: {Convert.ToBase64String(encryptedData)}\nIV: {Convert.ToBase64String(iv)}\nSalt: {Convert.ToBase64String(salt)}"
            );
            Console.WriteLine("Password Saved");
        }

        private static SQLiteConnection? ConnectToDb(byte[] key)
        {
            try
            {
                string filePath = @"passwords.db";
                string base64Key = Convert.ToBase64String(key);

                SQLiteConnection connection = new($"Data source={filePath};Password={base64Key}");

                Console.WriteLine("\nSuccessfully connected to db.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
                return null;
            }
        }

        private static bool AddEntry()
        {
            return true;
        }

        private static bool DeleteEntry()
        {
            return true;
        }
    }
}
