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
        public static Dictionary<string, DataTable> TransformFromDatabase(string entryName)
        {
            List<DataTable> tables = DatabaseHelper.LoadTablesForEntry(entryName);
            Dictionary<string,DataTable> result = new Dictionary<string, DataTable> ();
            List<string> tablenames = DatabaseHelper.GetTableNamesByEntryName(entryName);
            for (int i = 0; i < tables.Count; i++)
            {
                DataTable transformedTable = TransformDataTable(tables[i]);
                result[tablenames[i]] = transformedTable;
            }

            return result;
        }

        private static DataTable TransformDataTable(DataTable oldTableData)
        {
            DataTable newTableData = new DataTable();

            Dictionary<string, string> columnMappings = new Dictionary<string, string>(); // Alt -> Neu Zuordnung
            string timeUnit = "";

            // Neue Spalten basierend auf den alten Spalten definieren
            foreach (DataColumn column in oldTableData.Columns)
            {
                string columnName = column.ColumnName;
                if (columnName.Contains("Probe"))
                {
                    columnMappings[columnName] = columnName;
                    newTableData.Columns.Add(columnName, typeof(int));
                }
                else if (columnName.Contains("Zeit"))
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
            }
            columnMappings["Kommentar"] = "Kommentar";
            newTableData.Columns.Add("Kommentar", typeof(string));

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
