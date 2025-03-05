using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Data
{
    internal class DatabaseConnectionManager
    {
        private readonly string _connectionString;
        
        public DatabaseConnectionManager(bool isDebugMode, string databaseName)
        {
            string dbPath = isDebugMode
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName)
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), databaseName);

            _connectionString = $"Data Source={dbPath};Version=3;";

        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
