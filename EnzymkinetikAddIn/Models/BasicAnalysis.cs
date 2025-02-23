using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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

                // Neue Spalten hinzufügen
                if (!resultTable.Columns.Contains("Durchschnitt"))
                {
                    resultTable.Columns.Add("Durchschnitt", typeof(double));
                }
                if (!resultTable.Columns.Contains("Reale Konzentration"))
                {
                    resultTable.Columns.Add("Reale Konzentration", typeof (double));
                }

                foreach (DataRow row in resultTable.Rows)
                {

                }
                result[key] = resultTable;
            }


          return result;
        }
    }
}
