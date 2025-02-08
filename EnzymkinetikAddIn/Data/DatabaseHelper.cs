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
        public static void InitializeDatabaseConnection(bool isDebugMode)
        {
            string dbPath = isDebugMode
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "enzymkinetik.db")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "enzymkinetik.db");

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


        public static bool SaveDataGridViewToDatabase(DataGridView dgv, string baseTableName, bool editMode = false)
        {
            if (dgv == null || dgv.Rows.Count == 0)
                return false;

            List<string> tableNames = GetTableNames();

            using (var conn = GetConnection())
            {
                string tableName = baseTableName;
                conn.Open();

                if (!editMode && tableNames.Contains(tableName))
                {
                    tableName = GetUniqueTableName(conn, baseTableName);
                }

                StorageName = tableName;

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        CreateTable(conn, dgv, tableName);
                        InsertRows(conn, dgv, tableName);

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
        private static void CreateTable(SQLiteConnection conn, DataGridView dgv, string tableName)
        {
            var columnDefinitions = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                .Select(c => $"[{SanitizeColumnName(c.HeaderText)}] {GetSqlType(c.ValueType)}"));

            string createTableQuery = $"CREATE TABLE {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT, {columnDefinitions})";
            using (var cmd = new SQLiteCommand(createTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        // Zeilen in die Tabelle einfügen
        private static void InsertRows(SQLiteConnection conn, DataGridView dgv, string tableName)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var columnNames = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                    .Select(c => $"[{SanitizeColumnName(c.HeaderText)}]"));
                var values = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(_ => "?"));

                string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
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

        public static bool UpdateTableFromDataGridView(DataGridView dataGridView, string oldName, string tableName)
        {
            tableName = tableName.Replace(" ", "_");
            MessageBox.Show("old:" + oldName + "\nnew:" + tableName);
            if (!oldName.Equals(tableName))
            {
                RenameTable(oldName, tableName);
            }
            try
            {
                DeleteTable(tableName);
                SaveDataGridViewToDatabase(dataGridView, tableName);
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
    }
}
