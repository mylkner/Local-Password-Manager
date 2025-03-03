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
                Db.AddKeySalt(salt);
            }

            using Rfc2898DeriveBytes pbkdf2 = new(
                masterPassword,
                salt,
                100000,
                HashAlgorithmName.SHA512
            );

            Store.KeyManager.SetKey(pbkdf2.GetBytes(32));
        }

        public static (byte[] encryptedPassword, byte[] iv) EncryptPassword(
            string password,
            byte[] key
        )
        {
            byte[] iv = GenerateSalt(16);
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using MemoryStream ms = new();
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);

            cs.Write(passwordInBytes, 0, passwordInBytes.Length);
            cs.FlushFinalBlock();

            return (ms.ToArray(), iv);
        }

        public static string DecryptPassword(byte[] key, byte[] iv)
        {
            return "a";
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
