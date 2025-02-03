using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Data
{
    internal class DatabaseHelper
    {
        // Speichern in AppData
        //private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"enzymkinetik.db");
        
        // Speichern im Projektverzeichnis
        private static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "enzymkinetik.db");

        private static readonly string ConnectionString = $"Data Source={DbPath};Version=3;";

        static DatabaseHelper()
        {
            if (!File.Exists(DbPath))
            {
                CreateDatabase();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        private static void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbPath);
        }

        public static void SaveDataGridViewToDatabase(DataGridView dgv, string baseTableName)
        {
            if (dgv == null || dgv.Rows.Count == 0)
                return;

            using (var conn = GetConnection())
            {
                conn.Open();

                // Einen eindeutigen Tabellennamen generieren
                string tableName = GetUniqueTableName(conn, baseTableName);

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Tabelle erstellen
                        var columnDefinitions = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                            .Select(c => $"[{c.Name}] TEXT")); // Alle Spalten als TEXT speichern

                        string createTableQuery = $"CREATE TABLE {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT, {columnDefinitions})";
                        using (var cmd = new SQLiteCommand(createTableQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Daten einfügen
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue; // Leere letzte Zeile ignorieren

                            var columnNames = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(c => $"[{c.Name}]"));
                            var values = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(_ => "?"));

                            string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";

                            using (var cmd = new SQLiteCommand(insertQuery, conn))
                            {
                                foreach (DataGridViewColumn col in dgv.Columns)
                                {
                                    cmd.Parameters.AddWithValue($"@{col.Name}", row.Cells[col.Index].Value?.ToString() ?? DBNull.Value.ToString());
                                }
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show($"Daten wurden in die Tabelle '{tableName}' gespeichert.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Fehler beim Speichern: {ex.Message}");
                    }
                }
            }
        }

        public static string GetUniqueTableName(SQLiteConnection conn, string baseName)
        {
            int counter = 1;
            string tableName = baseName;

            using (var cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' AND name = @name", conn))
            {
                cmd.Parameters.AddWithValue("@name", tableName);

                while (cmd.ExecuteScalar() != null) // Prüft, ob die Tabelle existiert
                {
                    tableName = $"{baseName}_{counter}";
                    cmd.Parameters["@name"].Value = tableName;
                    counter++;
                }
            }

            return tableName;
        }
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
                        tableNames.Add(reader.GetString(0)); // Spaltenindex 0 enthält den Tabellennamen
                    }
                }
            }

            return tableNames;
        }
    }
}
