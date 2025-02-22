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

            bool dbExists = File.Exists(dbPath);

            if (!dbExists)
            {
                CreateDatabase(dbPath);
            }

            // Verbindung zur Datenbank öffnen und sicherstellen, dass die "Entries"-Tabelle existiert
            using (var conn = GetConnection())
            {
                conn.Open();
                EnsureEntriesTableExists(conn);
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
                int entryId = GetOrCreateEntryId(conn, entryName);
                string tableName = $"{entryName}_{tableSuffix}".Replace(" ", "_");

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        CreateTable(conn, dgv, tableName, entryId);
                        InsertRows(conn, dgv, tableName, entryId);

                        SaveTableName(conn, entryId, tableName); // Tabellennamen speichern

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

                // Spaltennamen und Platzhalter vorbereiten
                var columnNames = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                    .Select(c => $"[{SanitizeColumnName(c.HeaderText)}]"));
                var paramPlaceholders = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                    .Select((c, index) => $"@param{index}"));

                // SQL-Query erstellen
                string insertQuery = $"INSERT INTO {tableName} (EntryID, {columnNames}) VALUES (@entryId, {paramPlaceholders})";

                using (var cmd = new SQLiteCommand(insertQuery, conn))
                {
                    // Parameter für EntryID hinzufügen
                    cmd.Parameters.AddWithValue("@entryId", entryId);

                    // Parameter für die Zellen hinzufügen
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        object value = row.Cells[i].Value ?? DBNull.Value;
                        cmd.Parameters.AddWithValue($"@param{i}", ConvertToSqlValue(value) ?? DBNull.Value);
                    }

                    cmd.ExecuteNonQuery(); // SQL-Statement ausführen
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
        public static List<DataTable> LoadTablesForEntry(string entryName)
        {
            List<DataTable> dataTables = new List<DataTable>();

            using (var conn = GetConnection())
            {
                conn.Open();

                // Hole alle Tabellennamen, die zu diesem Entry gehören
                string query = @"
                SELECT TableName 
                FROM EntryTables 
                INNER JOIN Entries ON EntryTables.EntryID = Entries.ID 
                WHERE Entries.Name = @entryName;";

                List<string> tableNames = new List<string>();

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@entryName", entryName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableNames.Add(reader.GetString(0));
                        }
                    }
                }
                // Lade die Inhalte jeder Tabelle in ein DataTable
                foreach (string tableName in tableNames)
                {
                    DataTable dt = new DataTable();
                    var columnNames = new List<string>();

                    using (var cmd = new SQLiteCommand($"PRAGMA table_info([{tableName}]);", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["name"].ToString();
                            if (columnName != "ID" && columnName != "EntryID")
                                columnNames.Add(columnName);
                        }
                    }

                    if (!columnNames.Any())
                        throw new Exception($"Keine Spalten gefunden in Tabelle: {tableName}");

                    string selectQuery = $"SELECT {string.Join(", ", columnNames.Select(c => $"[{c}]"))} FROM [{tableName}]";

                    using (var cmd = new SQLiteCommand(selectQuery, conn))
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                        dt.TableName = tableName; // Speichert den Tabellennamen im DataTable
                    }

                    dataTables.Add(dt);
                }
            }
            MessageBox.Show("Tabellen in Datatables: " + dataTables.Count());
            return dataTables;
        }


        public static List<string> GetTableNamesByEntryName(string entryName)
        {
            List<string> tableNames = new List<string>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Hole die ID des Eintrags anhand des Namens
                string getIdQuery = "SELECT ID FROM Entries WHERE Name = @entryName;";
                using (var command = new SQLiteCommand(getIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@entryName", entryName);

                    object result = command.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine($"Eintrag mit Name '{entryName}' nicht gefunden.");
                        return tableNames; // Leere Liste zurückgeben
                    }

                    int entryID = Convert.ToInt32(result);

                    // Hole alle Tabellennamen aus EntryTables, die zu dieser ID gehören
                    string getTablesQuery = "SELECT TableName FROM EntryTables WHERE EntryID = @entryID;";
                    using (var getTablesCommand = new SQLiteCommand(getTablesQuery, connection))
                    {
                        getTablesCommand.Parameters.AddWithValue("@entryID", entryID);

                        using (var reader = getTablesCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tableNames.Add(reader.GetString(0)); // TableName auslesen und hinzufügen
                            }
                        }
                    }
                }

                connection.Close();
            }

            return tableNames;
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

        // TODO: Für neue Integration der Datenbank anpassen
        public static bool UpdateTableFromDataGridView(DataGridView dataGridView, string entryName, string tableSuffix)
        {
            string tableName = $"{entryName}_{tableSuffix}".Replace(" ", "_");
            MessageBox.Show("old: " + entryName + "\nnew: " + tableName);

            try
            {
                DeleteTable(tableName);
                SaveDataGridViewToDatabase(dataGridView, entryName, tableSuffix);
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
                Name TEXT NOT NULL UNIQUE
            );";
            using (var cmd = new SQLiteCommand(createEntriesTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }

            string createEntryTablesQuery = @"
            CREATE TABLE IF NOT EXISTS EntryTables (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                EntryID INTEGER NOT NULL,
                TableName TEXT NOT NULL,
                FOREIGN KEY (EntryID) REFERENCES Entries(ID) ON DELETE CASCADE
            );";


            using (var cmd = new SQLiteCommand(createEntryTablesQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteEntryWithTablesByName(string entryName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Hole die ID des Eintrags basierend auf dem Namen
                string getIdQuery = "SELECT ID FROM Entries WHERE Name = @entryName;";
                var command = new SQLiteCommand(getIdQuery, connection);
                command.Parameters.AddWithValue("@entryName", entryName);

                object result = command.ExecuteScalar();
                if (result == null)
                {
                    Console.WriteLine($"Eintrag mit Name '{entryName}' nicht gefunden.");
                    return;
                }

                int entryID = Convert.ToInt32(result);

                // Hole die Tabellennamen aus EntryTables
                string getTablesQuery = "SELECT TableName FROM EntryTables WHERE EntryID = @entryID;";
                command = new SQLiteCommand(getTablesQuery, connection);
                command.Parameters.AddWithValue("@entryID", entryID);

                List<string> tableNames = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                }

                // Lösche den Eintrag aus Entries (CASCADE löscht automatisch die Einträge in EntryTables)
                string deleteEntryQuery = "DELETE FROM Entries WHERE ID = @entryID;";
                command = new SQLiteCommand(deleteEntryQuery, connection);
                command.Parameters.AddWithValue("@entryID", entryID);
                command.ExecuteNonQuery();

                // Lösche alle Tabellen, die im Feld TableName standen
                foreach (string tableName in tableNames)
                {
                    string dropTableQuery = $"DROP TABLE IF EXISTS {tableName};";
                    command = new SQLiteCommand(dropTableQuery, connection);
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Tabelle '{tableName}' gelöscht.");
                }

                connection.Close();
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

                string query = @"
                SELECT TableName 
                FROM EntryTables 
                INNER JOIN Entries ON EntryTables.EntryID = Entries.ID 
                WHERE Entries.Name = @entryName;";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@entryName", entryName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableNames.Add(reader.GetString(0));
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

        private static void SaveTableName(SQLiteConnection conn, int entryId, string tableName)
        {
            string insertQuery = "INSERT INTO EntryTables (EntryID, TableName) VALUES (@entryId, @tableName);";

            using (var cmd = new SQLiteCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@entryId", entryId);
                cmd.Parameters.AddWithValue("@tableName", tableName);
                cmd.ExecuteNonQuery();
            }
        }



    }
}
