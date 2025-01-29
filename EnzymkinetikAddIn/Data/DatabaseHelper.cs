using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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
    }
}
