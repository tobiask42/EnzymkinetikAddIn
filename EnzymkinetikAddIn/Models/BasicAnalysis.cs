using System;
using System.Collections.Generic;
using System.Data;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Models
{
    internal class BasicAnalysis : IModelLogic
    {
        public Dictionary<string, DataTable> CalculateResult(string entryName)
        {
            Dictionary<string, DataTable> rawData = DataTransformer.TransformFromDatabase(entryName);
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            foreach (var kvp in rawData)
            {
                string key = kvp.Key;
                DataTable data = kvp.Value;
                DataTable newTable = data.Copy();

                int totalColumns = newTable.Columns.Count;
                int numberOfConcentrations = (totalColumns - 3) / 3; // Anzahl der Konzentrationen berechnen

                for (int i = 0; i <= numberOfConcentrations; i++)
                {
                    int baseIndex = 2 + i * 3; // Index der ersten Spalte pro Konzentration
                    int dilutionIndex = baseIndex;
                    int measurement1Index = baseIndex + 1;
                    int measurement2Index = baseIndex + 2;

                    string concentrationLabel = $"c_{i + 1}";
                    string avg = $"{concentrationLabel} Durchschnitt";
                    string undiluted = $"{concentrationLabel} Unverdünnt";

                    // Spalten hinzufügen
                    newTable.Columns.Add(avg, typeof(double));
                    newTable.Columns.Add(undiluted, typeof(double));

                    // Berechnungen durchführen
                    foreach (DataRow row in newTable.Rows)
                    {
                        double mw1 = row[measurement1Index] != DBNull.Value ? Convert.ToDouble(row[measurement1Index]) : double.NaN;
                        double mw2 = row[measurement2Index] != DBNull.Value ? Convert.ToDouble(row[measurement2Index]) : double.NaN;
                        double dilution = row[dilutionIndex] != DBNull.Value ? Convert.ToDouble(row[dilutionIndex]) : double.NaN;

                        row[avg] = (!double.IsNaN(mw1) && !double.IsNaN(mw2)) ? (mw1 + mw2) / 2 : double.NaN;
                        row[undiluted] = (!double.IsNaN((double)row[avg]) && !double.IsNaN(dilution))
                            ? (double)row[avg] * dilution
                            : double.NaN;
                    }

                    // Spalten direkt nach den Messwerten verschieben
                    int insertIndex = baseIndex + 3; // Direkt nach Messwert 2
                    //MoveColumn(newTable, avg, insertIndex);
                    //MoveColumn(newTable, undiluted, insertIndex + 1);
                }

                result[key] = newTable;
            }

            return result;
        }

        /// <summary>
        /// Verschiebt eine Spalte an eine bestimmte Position.
        /// </summary>
        private void MoveColumn(DataTable table, string columnName, int newPosition)
        {
            if (table.Columns.Contains(columnName))
            {
                table.Columns[columnName].SetOrdinal(newPosition);
            }
            else
            {
                throw new ArgumentException($"Die Spalte '{columnName}' existiert nicht in der Tabelle.");
            }
        }
    }
}
