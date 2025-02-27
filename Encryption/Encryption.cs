using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    class EncryptUtils
    {
        public static (byte[] salt, byte[] key) DeriveKeyFromMasterPassword(string masterPassword)
        {
            byte[] salt = GenerateSalt(32);

            using Rfc2898DeriveBytes pbkdf2 = new(
                masterPassword,
                salt,
                100000, //iterations
                HashAlgorithmName.SHA512
            );

            return (salt, pbkdf2.GetBytes(32));
        }

        private static byte[] GenerateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }

        public static (byte[] encryptedData, byte[] iv) EncryptPassword(string password, byte[] key)
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

        private static string RandomPasswordGenerator()
        {
            return "a";
        }
    }
}
