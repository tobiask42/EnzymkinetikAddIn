using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Data;

namespace EnzymkinetikAddIn.Utilities
{
    internal class DataTransformer
    {
        public static DataTable TransformFromDatabase(string tableName)
        {
            DataTable oldTableData = DatabaseHelper.LoadTable(tableName);
            DataTable newTableData = new DataTable();

            Dictionary<string, string> columnMappings = new Dictionary<string, string>(); // Alt -> Neu Zuordnung
            string timeUnit = "";

            // Neue Spalten basierend auf den alten Spalten definieren
            foreach (DataColumn column in oldTableData.Columns)
            {
                string columnName = column.ColumnName;

                if (columnName.Contains("Zeit"))
                {
                    timeUnit = Char.ToString(columnName.ElementAt(6)) == "m" ? "min" : "s";
                    string newColumnName = "Zeit (" + timeUnit + ")";
                    newTableData.Columns.Add(newColumnName, typeof(double));
                    columnMappings[columnName] = newColumnName; // Alt -> Neu
                }
                else if (columnName.Contains("Verdünnung"))
                {
                    string number = Char.ToString(columnName.ElementAt(2));
                    string newColumnName = "Verdünnung " + number;
                    newTableData.Columns.Add(newColumnName, typeof(double));
                    columnMappings[columnName] = newColumnName;
                }
                else if (columnName.Contains("Messwert"))
                {
                    char c = columnName.ElementAt(2);
                    char num = columnName.ElementAt(13);
                    string sampleUnit = columnName.Contains("g_L") ? "g/L" : "mg/dL";
                    string newColumnName = num == '1' ? $"Messwert 1 (c_{c}, {sampleUnit})" : $"Messwert 2 (c_{c}, {sampleUnit})";
                    newTableData.Columns.Add(newColumnName, typeof(double));
                    columnMappings[columnName] = newColumnName;
                }
                else
                {
                    string newColumnName = "Kommentar";
                    newTableData.Columns.Add(newColumnName, typeof(string));
                    columnMappings[columnName] = newColumnName;
                }
            }
            // Daten spaltenweise übertragen
            int rowCount = oldTableData.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                DataRow newRow = newTableData.NewRow();

                foreach (var mapping in columnMappings)
                {
                    string oldColumn = mapping.Key;
                    string newColumn = mapping.Value;
                    object oldValue = oldTableData.Rows[i][oldColumn];

                    // NULL-Check für verschiedene Datentypen
                    if (oldValue == DBNull.Value)
                    {
                        if (newTableData.Columns[newColumn].DataType == typeof(double))
                        {
                            newRow[newColumn] = double.NaN;  // Leere Werte als NaN für Double
                        }
                        else
                        {
                            newRow[newColumn] = ""; // Leere Strings für Textspalten
                        }
                    }
                    else
                    {
                        newRow[newColumn] = oldValue;
                    }
                }

                newTableData.Rows.Add(newRow);
            }

            return newTableData;
        }

    }
}
