using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public static class EncryptUtils
    {
        public static string CreateMasterPassword()
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

            if (masterPassword == null)
                throw new ArgumentNullException("Master password cannot be null");

            Console.WriteLine("\nPassword set.");
            return masterPassword;
        }

        public static byte[] DeriveKeyFromMasterPassword(string masterPassword)
        {
            Console.WriteLine("Creating key...");
            byte[] salt = GenerateSalt(32);

            using Rfc2898DeriveBytes pbkdf2 = new(
                masterPassword,
                salt,
                100000,
                HashAlgorithmName.SHA512
            );

            byte[] key = pbkdf2.GetBytes(32);

            return key;
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

        private static byte[] GenerateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }

        private static string RandomPasswordGenerator()
        {
            return default;
        }
    }
}
