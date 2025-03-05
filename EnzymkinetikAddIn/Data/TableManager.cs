using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Data
{
    internal class TableManager
    {
        private readonly SQLiteConnection _connection;

        public TableManager(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTable(DataGridView dgv, string tableName)
        {
            // Prüfen, ob die Tabelle existiert und gegebenenfalls löschen
            string checkIfTableExistsQuery = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            using (var cmd = new SQLiteCommand(checkIfTableExistsQuery, _connection))
            {
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    // Tabelle löschen, wenn sie existiert (optional, je nach Anforderung)
                    string dropTableQuery = $"DROP TABLE IF EXISTS {tableName}";
                    using (var dropCmd = new SQLiteCommand(dropTableQuery, _connection))
                    {
                        dropCmd.ExecuteNonQuery();
                    }
                }
            }

            // Spaltendefinitionen aus DataGridView generieren
            var columnDefinitions = string.Join(", ", dgv.Columns.Cast<DataGridViewColumn>()
                .Select(c => $"[{DatabaseUtils.SanitizeColumnName(c.HeaderText)}] {DatabaseUtils.GetSqlType(c.ValueType)}"));

            // SQL-Query zum Erstellen der Tabelle
            string createTableQuery = $@"
                CREATE TABLE @tablename (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    EntryID INTEGER NOT NULL,
                    {columnDefinitions},
                    FOREIGN KEY (EntryID) REFERENCES Entries(ID) ON DELETE CASCADE
                );";

            using (var cmd = new SQLiteCommand(createTableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@tablename", tableName);
                cmd.ExecuteNonQuery(); // Tabelle erstellen
            }
        }
    }
}
