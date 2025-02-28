using Microsoft.Data.Sqlite;

namespace SQLite
{
    public static class Db
    {
        private static readonly string _filePath = "passwords.db";
        private static SqliteConnection? _connection;

        public static bool CheckIfDbExists()
        {
            return File.Exists(_filePath);
        }

        public static void CreateConnection(byte[] key, bool initial)
        {
            if (initial)
            {
                Console.WriteLine("Attempting database creation and connection...");
            }
            else
            {
                Console.WriteLine("Attempting to connect to database...");
            }

            try
            {
                using SqliteConnection cnn = new($"Data Source={_filePath};Password={key}");
                _connection = cnn;
                Console.WriteLine("Succesfully connected to database.");
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
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
