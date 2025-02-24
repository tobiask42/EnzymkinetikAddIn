using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Models
{
    internal class BasicAnalysis : IModelLogic
    {
        public Dictionary<string, DataTable> CalculateResult(string entryName)
        {
            // MessageBox.Show("Model: Basic Analysis");
            Dictionary<string, DataTable> rawData = DataTransformer.TransformFromDatabase(entryName);
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            foreach (var kvp in rawData)
            {
                string key = kvp.Key;
                DataTable data = kvp.Value;
                int size = data.Columns.Count;
                // Neue DataTable für Ergebnisse
                DataTable resultTable = data.Copy();



                int totalColumns = data.Columns.Count;
                // Gesamtspalten - 4 (Probe, Zeit, erste Verdünnung, Kommentar) geteilt durch 3 ergibt n
                int numberOfConcentrations = (totalColumns - 4) / 3;

                for (int i = 0; i <= numberOfConcentrations; i++)
                {
                    int baseIndex = 2 + i * 3; // Startindex für jede Konzentration: Index 2 ist die erste Verdünnung

                    int verdünnungIndex = baseIndex;
                    int messwert1Index = baseIndex + 1;
                    int messwert2Index = baseIndex + 2;

                    string concentrationLabel = $"c_{i + 1}";
                    string durchschnitt = $"{concentrationLabel} Durchschnitt";
                    string unverdünnt = $"{concentrationLabel} Unverdünnt";

                    resultTable.Columns.Add(durchschnitt, typeof(double));
                    resultTable.Columns.Add(unverdünnt, typeof(double));

                    foreach (DataRow row in resultTable.Rows)
                    {
                        double mw1 = row[messwert1Index] != DBNull.Value ? Convert.ToDouble(row[messwert1Index]) : double.NaN;
                        double mw2 = row[messwert2Index] != DBNull.Value ? Convert.ToDouble(row[messwert2Index]) : double.NaN;
                        double dilution = row[verdünnungIndex] != DBNull.Value ? Convert.ToDouble(row[verdünnungIndex]) : double.NaN;

                        row[durchschnitt] = (!double.IsNaN(mw1) && !double.IsNaN(mw2)) ? (mw1 + mw2) / 2 : double.NaN;
                        row[unverdünnt] = (!double.IsNaN((double)row[durchschnitt]) && !double.IsNaN(dilution))
                            ? (double)row[durchschnitt] * dilution
                            : double.NaN;
                    }
                }
                //resultTable = MoveColumns(resultTable, size);

                result[key] = resultTable;
            }
            return result;
        }

        private DataTable MoveColumns(DataTable resultTable, int baseSize)
        {
            int cGroups = (baseSize - 3) / 3; // cGroups ist die Anzahl der c-Gruppen
            List<int> from = new List<int>();  // Liste der Spalten, die verschoben werden
            List<int> to = new List<int>();    // Zielpositionen für die Spalten

            // Durchlaufen der c-Gruppen
            for (int i = 0; i < cGroups; i++)
            {

            }

            // Ausgabe der Indizes vor der Verschiebung zur Kontrolle
            MessageBox.Show("From: " + string.Join(", ", from.ToArray()) +
                             "\nTo: " + string.Join(", ", to.ToArray()));

            // Verschieben der Spalten in die richtige Reihenfolge
            DataTableUtils.MoveColumns(resultTable, from.ToArray(), to.ToArray());

            return resultTable;
        }



    }
}