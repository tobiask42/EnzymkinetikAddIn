using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

                // Neue DataTable für Ergebnisse
                DataTable resultTable = data.Copy();


                // Alle Konzentrationen ermitteln (c_1, c_2, c_3,...,c_n)
                var concentrationGroups = resultTable.Columns.Cast<DataColumn>()
                    .Select(c => Regex.Match(c.ColumnName, @"c_(\d+)"))  // Extrahiere c_1, c_2,...,c_n
                    .Where(m => m.Success)
                    .Select(m => m.Value)
                    .Distinct()
                    .ToList();

                // Für jede Konzentration neue Spalten hinzufügen
                foreach (string concentration in concentrationGroups)
                {
                    string avgColumn = $"{concentration} Durchschnitt";
                    string undilutedColumn = $"{concentration} Unverdünnt";

                    if (!resultTable.Columns.Contains(avgColumn))
                        resultTable.Columns.Add(avgColumn, typeof(double));

                    if (!resultTable.Columns.Contains(undilutedColumn))
                        resultTable.Columns.Add(undilutedColumn, typeof(double));
                }

                foreach (DataRow row in resultTable.Rows)
                {
                    foreach (string concentration in concentrationGroups)
                    {
                        // Verdünnungsfaktor ermitteln
                        double dilutionFactor = double.NaN;
                        DataColumn dilutionColumn = resultTable.Columns.Cast<DataColumn>()
                            .FirstOrDefault(c => Regex.IsMatch(c.ColumnName, $@"{concentration}.*verdünnung", RegexOptions.IgnoreCase));
                        if (dilutionColumn != null && row[dilutionColumn] != DBNull.Value)
                        {
                            dilutionFactor = Convert.ToDouble(row[dilutionColumn]);
                        }
                        else
                        {
                            dilutionFactor = 1.0;
                        }
                        // Messwerte finden und als Liste speichern
                        var measurementColumns = resultTable.Columns.Cast<DataColumn>()
                            .Where(c => Regex.IsMatch(c.ColumnName, $@"{concentration}.*messwert", RegexOptions.IgnoreCase))
                            .ToList();
                        var measurements = measurementColumns
                            .Select(c => row[c] != DBNull.Value ? Convert.ToDouble(row[c]):double.NaN)
                            .Where(v => !double.IsNaN(v))
                            .ToList();
                        // Durchschnitt berechnen
                        double avg = measurements.Any() ? measurements.Average() : double.NaN;
                        // Unverdünnte Konzentration = Durchschnitt * Verdünnungsfaktor
                        double undiluted = !double.IsNaN(avg) ? avg * dilutionFactor : double.NaN;
                    }
                }
                result[key] = resultTable;
            }
          return result;
        }
    }
}
