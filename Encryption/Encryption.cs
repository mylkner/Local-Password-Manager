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
            using StreamReader sr = new(cs);

            cs.Write(passwordInBytes, 0, passwordInBytes.Length);
            cs.FlushFinalBlock();

            return (ms.ToArray(), iv);
        }

        public static string DecryptPassword(byte[] encryptedPassword, byte[] iv)
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

        private static byte[] GenerateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }

        private static string RandomPasswordGenerator()
        {
            return "a";
        }
    }
}
