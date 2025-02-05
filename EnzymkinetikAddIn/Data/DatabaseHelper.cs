using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;

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

                string tableName = GetUniqueTableName(conn, baseTableName);

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Bestimme die Datentypen basierend auf den Spalten
                        var columnDefinitions = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                            .Select(c => $"[{c.Name}] {GetSqlType(c.ValueType)}"));

                        string createTableQuery = $"CREATE TABLE {tableName} (ID INTEGER PRIMARY KEY AUTOINCREMENT, {columnDefinitions})";
                        using (var cmd = new SQLiteCommand(createTableQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Daten einfügen
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue;

                            var columnNames = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(c => $"[{c.Name}]"));
                            var values = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>().Select(_ => "?"));

                            string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";

                            using (var cmd = new SQLiteCommand(insertQuery, conn))
                            {
                                foreach (DataGridViewColumn col in dgv.Columns)
                                {
                                    object value = row.Cells[col.Index].Value ?? DBNull.Value;
                                    cmd.Parameters.AddWithValue($"@{col.Name}", ConvertToSqlValue(value));
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

        private static string GetSqlType(Type type)
        {
            if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte))
                return "INTEGER";
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return "REAL";
            if (type == typeof(bool))
                return "BOOLEAN";
            if (type == typeof(DateTime))
                return "TEXT"; // SQLite speichert Datumswerte als TEXT im ISO 8601-Format
            return "TEXT"; // Standardmäßig als TEXT speichern
        }

        private static object ConvertToSqlValue(object value)
        {
            if (value is bool boolVal)
                return boolVal ? 1 : 0; // SQLite speichert BOOLEAN als INTEGER (0/1)
            if (value is DateTime dateVal)
                return dateVal.ToString("yyyy-MM-dd HH:mm:ss"); // Einheitliches ISO 8601-Format für Datumswerte
            return value; // Alle anderen Werte unverändert lassen
        }
        public static void LoadTableToDataGridView(DataGridView dgv, string tableName)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = $"SELECT * FROM [{tableName}]";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                }
            }
        }

        public static DataTable LoadTable(string tableName)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand($"SELECT * FROM [{tableName}]", conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static void SaveTable(DataTable dt, string tableName)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    using (var adapter = new SQLiteDataAdapter($"SELECT * FROM [{tableName}]", conn))
                    {
                        var commandBuilder = new SQLiteCommandBuilder(adapter);
                        adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                        adapter.InsertCommand = commandBuilder.GetInsertCommand();
                        adapter.DeleteCommand = commandBuilder.GetDeleteCommand();

                        adapter.Update(dt);
                    }

                    transaction.Commit();
                }
            }
        }

    }

    
}
