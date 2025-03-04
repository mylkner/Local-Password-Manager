using System.Security.Cryptography;
using System.Text;
using SQLiteDB;

namespace Encryption
{
    public static class EncryptUtils
    {
        public static void DeriveKeyFromMasterPassword(string masterPassword, byte[]? salt)
        {
            if (salt == null)
            {
                salt = GenerateSalt(32);
                Db.SetUpDbWithKeySalt(salt);
            }

            using Rfc2898DeriveBytes pbkdf2 = new(
                masterPassword,
                salt,
                100000,
                HashAlgorithmName.SHA512
            );

            Store.KeyManager.SetKey(pbkdf2.GetBytes(32));
        }

        public static (byte[] encryptedPassword, byte[] iv) EncryptPassword(string password)
        {
            byte[] iv = GenerateSalt(16);
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);

            using Aes aes = Aes.Create();
            aes.Key = Store.KeyManager.GetKey();
            aes.IV = iv;

            using MemoryStream ms = new();
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);

            cs.Write(passwordInBytes, 0, passwordInBytes.Length);
            cs.FlushFinalBlock();

            return (ms.ToArray(), iv);
        }

        public static string DecryptPassword(byte[] iv, byte[] encryptedPassword)
        {
            using Aes aes = Aes.Create();
            aes.Key = Store.KeyManager.GetKey();
            aes.IV = iv;

            using MemoryStream ms = new(encryptedPassword);
            using ICryptoTransform decryptor = aes.CreateDecryptor();
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);

            return sr.ReadToEnd();
        }

        public static string RandomPasswordGenerator()
        {
            string validChars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+";
            Random rnd = new();
            int length = rnd.Next(16, 21);
            char[] password = new char[length];
            byte[] randomBytes = new byte[length];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[randomBytes[i] % validChars.Length];
            }

            return new string(password);
        }

        private static byte[] GenerateSalt(int size)
        {
            byte[] salt = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
