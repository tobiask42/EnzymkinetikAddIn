using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace EnzymkinetikAddIn.Data
{
    internal class DatabaseHelper
    {
        private static string _connectionString = string.Empty;

        public static string StorageName { get; private set; }


        // Datenbankverbindungs-String initialisieren
        public static void InitializeDatabaseConnection(bool isDebugMode, string databaseName)
        {
            string dbPath = isDebugMode
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName)
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), databaseName);

            _connectionString = $"Data Source={dbPath};Version=3;";

            if (!File.Exists(dbPath))
            {
                CreateDatabase(dbPath);
            }
        }

        // SQLite-Verbindung zurückgeben
        private static SQLiteConnection GetConnection() => new SQLiteConnection(_connectionString);

        // Neue Datenbank erstellen
        private static void CreateDatabase(string dbPath) => SQLiteConnection.CreateFile(dbPath);


        public static bool SaveDataGridViewToDatabase(DataGridView dgv, string entryName, string tableSuffix, bool editMode = false)
        {
            if (dgv == null || dgv.Rows.Count == 0)
                return false;

            using (var conn = GetConnection())
            {
                conn.Open();
                int entryId = GetOrCreateEntryId(conn, entryName); // Hier wird EntryID ermittelt

                string tableName = $"{entryName}_{tableSuffix}".Replace(" ", "_");

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        CreateTable(conn, dgv, tableName, entryId);
                        InsertRows(conn, dgv, tableName, entryId); // Jetzt mit entryId

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Fehler: {ex.Message}", "Datenbankfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }



        // Tabelle mit den Spalten aus DataGridView erstellen
        private static void CreateTable(SQLiteConnection conn, DataGridView dgv, string tableName, int entryId)
        {
            var columnDefinitions = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                .Select(c => $"[{SanitizeColumnName(c.HeaderText)}] {GetSqlType(c.ValueType)}"));

            string createTableQuery = $@"
        CREATE TABLE {tableName} (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            EntryID INTEGER NOT NULL,
            {columnDefinitions},
            FOREIGN KEY (EntryID) REFERENCES Entries(ID) ON DELETE CASCADE
        );";
            using (var cmd = new SQLiteCommand(createTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }


        // Zeilen in die Tabelle einfügen
        private static void InsertRows(SQLiteConnection conn, DataGridView dgv, string tableName, int entryId)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var columnNames = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                    .Select(c => $"[{SanitizeColumnName(c.HeaderText)}]"));
                var values = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(_ => "?"));

                string insertQuery = $"INSERT INTO {tableName} (EntryID, {columnNames}) VALUES (@entryId, {values})";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@entryId", entryId);
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        object value = row.Cells[col.Index].Value ?? DBNull.Value;
                        cmd.Parameters.AddWithValue($"@{SanitizeColumnName(col.HeaderText)}", ConvertToSqlValue(value));
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }





        // Spaltennamen für SQLite validieren
        private static string SanitizeColumnName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName)) return "Column";

            string sanitized = Regex.Replace(columnName, @"[^\w]", "_");

            if (char.IsDigit(sanitized[0])) sanitized = "_" + sanitized;

            return sanitized;
        }

        // SQL-Datentyp aus C#-Typen ableiten
        private static string GetSqlType(Type type)
        {
            if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte)) return "INTEGER";
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "REAL";
            if (type == typeof(bool)) return "BOOLEAN";
            if (type == typeof(DateTime)) return "TEXT";
            return "TEXT";
        }

        // Umwandlung eines C#-Werts in SQL-kompatiblen Wert
        private static object ConvertToSqlValue(object value)
        {
            if (value is bool boolVal) return boolVal ? 1 : 0;
            if (value is DateTime dateVal) return dateVal.ToString("yyyy-MM-dd HH:mm:ss");
            return value;
        }

        // Alle Tabellennamen abfragen
        public static List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();

            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }

            return tableNames;
        }

        // Tabelle mit einzigartigem Namen finden
        private static string GetUniqueTableName(SQLiteConnection conn, string baseName)
        {
            int counter = 1;
            string tableName = baseName;

            using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name = @name", conn))
            {
                cmd.Parameters.AddWithValue("@name", tableName);

                while (cmd.ExecuteScalar() != null)
                {
                    tableName = $"{baseName}_{counter}";
                    cmd.Parameters["@name"].Value = tableName;
                    counter++;
                }
            }

            return tableName;
        }


        // Tabelle laden und zurückgeben
        public static DataTable LoadTable(string tableName)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                tableName = tableName.Replace(" ", "_");

                var columnNames = new List<string>();
                using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader["name"].ToString();
                        if (columnName != "ID") columnNames.Add(columnName);
                    }
                }

                string selectQuery = $"SELECT {string.Join(", ", columnNames)} FROM [{tableName}]";

                using (var cmd = new SQLiteCommand(selectQuery, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // Tabelle umbenennen
        public static void RenameTable(string oldName, string newName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                newName = GetUniqueTableName(connection, newName);

                string query = $"ALTER TABLE {oldName} RENAME TO {newName};";
                var command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public static bool UpdateTableFromDataGridView(DataGridView dataGridView, string entryName, string tableSuffix)
        {
            string tableName = $"{entryName}_{tableSuffix}".Replace(" ", "_");
            MessageBox.Show("old: " + entryName + "\nnew: " + tableName);

            try
            {
                DeleteTable(tableName);
                SaveDataGridViewToDatabase(dataGridView, entryName, tableSuffix); // Fix: tableSuffix hinzugefügt
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        // Tabelle löschen
        public static void DeleteTable(string tableName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = $"DROP TABLE IF EXISTS [{tableName}];";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void EnsureEntriesTableExists(SQLiteConnection conn)
        {
            string createEntriesTableQuery = @"
        CREATE TABLE IF NOT EXISTS Entries (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT UNIQUE NOT NULL
        );";
            using (var cmd = new SQLiteCommand(createEntriesTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static int GetOrCreateEntryId(SQLiteConnection conn, string entryName)
        {
            EnsureEntriesTableExists(conn);

            string selectQuery = "SELECT ID FROM Entries WHERE Name = @name;";
            using (var cmd = new SQLiteCommand(selectQuery, conn))
            {
                cmd.Parameters.AddWithValue("@name", entryName);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            string insertQuery = "INSERT INTO Entries (Name) VALUES (@name);";
            using (var cmd = new SQLiteCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@name", entryName);
                cmd.ExecuteNonQuery();
            }

            return (int)conn.LastInsertRowId;
        }

        public static List<string> GetTablesForEntry(string entryName)
        {
            List<string> tableNames = new List<string>();

            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT name FROM sqlite_master WHERE type='table' AND name LIKE @entryNamePattern";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@entryNamePattern", entryName + "_%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            tableNames.Add(tableName.Replace(entryName + "_", "")); // Suffix extrahieren
                        }
                    }
                }
            }

            return tableNames;
        }


        public static List<string> GetEntryNames()
        {
            List<string> entryNames = new List<string>();

            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT Name FROM Entries";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entryNames.Add(reader.GetString(0));
                    }
                }
            }

            return entryNames;
        }
    }
}
