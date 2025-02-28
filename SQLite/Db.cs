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

        public static void CreateConnection(string password)
        {
            try
            {
                using SqliteConnection cnn = new($"Data Source={_filePath};Password={password}");
                _connection = cnn;
                cnn.Open();
            }
            catch
            {
                Console.WriteLine("Database connection failed.");
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
