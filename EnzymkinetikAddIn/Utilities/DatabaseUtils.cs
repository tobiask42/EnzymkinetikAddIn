using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Utilities
{
    internal class DatabaseUtils
    {
        public static string SanitizeColumnName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName)) return "Column";

            string sanitized = Regex.Replace(columnName, @"[^\w]", "_");

            if (char.IsDigit(sanitized[0])) sanitized = "_" + sanitized;

            return sanitized;
        }

        public static string GetSqlType(Type type)
        {
            if (type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte)) return "INTEGER";
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "REAL";
            if (type == typeof(bool)) return "BOOLEAN";
            if (type == typeof(DateTime)) return "TEXT";
            return "TEXT";
        }
    }
}
