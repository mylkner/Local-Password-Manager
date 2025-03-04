using Microsoft.Data.Sqlite;

namespace SQLiteDB
{
    public static class Db
    {
        private static readonly string _filePath = "passwords.db";
        private static SqliteConnection? _conn;

        public static bool DbExists() => File.Exists(_filePath);

        public static byte[]? OpenDb(string password)
        {
            using var conn = new SqliteConnection($"Data Source={_filePath};");
            conn.Open();

            using var addKeyCmd = conn.CreateCommand();
            addKeyCmd.CommandText = $"PRAGMA key = `{password}`";
            addKeyCmd.ExecuteNonQuery();

            _conn = conn;
            return GetKeySalt();
        }

        public static void SetUpDbWithKeySalt(byte[] salt)
        {
            using SqliteCommand createPwdTabCmd = GetConnection().CreateCommand();
            createPwdTabCmd.CommandText =
                "CREATE TABLE IF NOT EXISTS passwords (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT NOT NULL UNIQUE, iv BLOB NOT NULL, encryptedPassword BLOB NOT NULL)";
            createPwdTabCmd.ExecuteNonQuery();

            using SqliteCommand createSaltTabCmd = GetConnection().CreateCommand();
            createSaltTabCmd.CommandText =
                "CREATE TABLE IF NOT EXISTS key_salt (id INTEGER PRIMARY KEY CHECK (id = 1), salt BLOB NOT NULL)";
            createSaltTabCmd.ExecuteNonQuery();

            using SqliteCommand insertCmd = GetConnection().CreateCommand();
            insertCmd.CommandText = "INSERT INTO key_salt (id, salt) VALUES (1, @salt)";
            insertCmd.Parameters.AddWithValue("@salt", salt);
            insertCmd.ExecuteNonQuery();
        }

        public static void GetEntries()
        {
            using SqliteCommand getCmd = GetConnection().CreateCommand();
            getCmd.CommandText = "SELECT title, iv, encryptedPassword FROM passwords";
            SqliteDataReader reader = getCmd.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine("No entries found.");
                return;
            }

            while (reader.Read())
            {
                string title = reader.GetString(0);
                Console.WriteLine(title);
            }
        }

        public static void AddEntry(string title, byte[] iv, byte[] encryptedPassword)
        {
            using SqliteCommand insertCmd = GetConnection().CreateCommand();
            insertCmd.CommandText =
                "INSERT INTO passwords (title, iv, encryptedPassword) VALUES (@title, @iv, @encryptedPassword)";
            insertCmd.Parameters.AddWithValue("@title", title);
            insertCmd.Parameters.AddWithValue("@iv", iv);
            insertCmd.Parameters.AddWithValue("@encryptedPassword", encryptedPassword);
            insertCmd.ExecuteNonQuery();
        }

        public static void DeleteEntry(string title)
        {
            using SqliteCommand deleteCmd = GetConnection().CreateCommand();
            deleteCmd.CommandText = $"DELETE FROM passwords WHERE title = {title}";
            deleteCmd.ExecuteNonQuery();
        }

        private static SqliteConnection GetConnection()
        {
            if (_conn == null)
                throw new ArgumentNullException(null, "Connection cannot be null");
            _conn.Open();
            return _conn;
        }

        private static byte[]? GetKeySalt()
        {
            try
            {
                using SqliteCommand getCmd = GetConnection().CreateCommand();
                getCmd.CommandText = "SELECT salt FROM key_salt WHERE id = 1";
                object? result = getCmd.ExecuteScalar();

                return (byte[]?)result;
            }
            catch
            {
                return null;
            }
        }
    }
}
