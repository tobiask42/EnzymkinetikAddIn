using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;
using Microsoft.Office.Interop.Excel;

namespace EnzymkinetikAddIn.Logic
{
    internal class CalibrationLogic : IModelLogic
    {
        public void ConfigureColumns(DataGridView dataGridView)
        {
            var columnManager = new ColumnManager(dataGridView);

            columnManager.InitializeTimeColumn();
            columnManager.InitializeIntCol("dilution", "Verdünnung");
            columnManager.InitializeDoubleCol("reading_1", "Messwert 1");
            columnManager.InitializeDoubleCol("reading_2", "Messwert 2");
            columnManager.InitializeDoubleCol("average", "Durchschnitt", readOnly:true);
            columnManager.InitializeDoubleCol("undiluted", "Unverdünnt", readOnly: true);
            columnManager.InitializeBaseCol("comment", "Kommentar");


            // Event für automatische Berechnung hinzufügen
            dataGridView.CellValueChanged += (sender, e) =>
            {
                // Berechnung nur für relevante Spalten durchführen
                if (e.ColumnIndex == dataGridView.Columns["reading_1"].Index ||
                    e.ColumnIndex == dataGridView.Columns["reading_2"].Index ||
                    e.ColumnIndex == dataGridView.Columns["dilution"].Index)
                {
                    PerformCalculations(dataGridView);
                }
            };

            // Bearbeitung abschließen, um Änderungen zu registrieren
            dataGridView.CellEndEdit += (sender, e) =>
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        public void PerformCalculations(DataGridView dataGridView)
        {
            CalculateAverage(dataGridView);
            CalculateUndiluted(dataGridView);
        }

        public void CalculateAverage(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var reading1 = row.Cells["reading_1"].Value;
                var reading2 = row.Cells["reading_2"].Value;

                // Überprüfen, ob die Werte gültige Zahlen sind
                if (reading1 != null && reading2 != null
                    && double.TryParse(reading1.ToString(), out double value1)
                    && double.TryParse(reading2.ToString(), out double value2))
                {
                    row.Cells["average"].Value = (value1 + value2) / 2;
                }
                else
                {
                    // Falls ungültige Eingaben vorliegen, setzen wir den Wert auf null
                    row.Cells["average"].Value = null;
                }
            }
        }

        public void CalculateUndiluted(DataGridView dataGridView)
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var dilution = row.Cells["dilution"].Value;
                var average = row.Cells["average"].Value;

                // Überprüfen, ob die Werte gültige Zahlen sind
                if (dilution != null && average != null
                    && double.TryParse(dilution.ToString(), out double dilutionValue)
                    && double.TryParse(average.ToString(), out double averageValue))
                {
                    row.Cells["undiluted"].Value = dilutionValue * averageValue;
                }
                else
                {
                    // Falls ungültige Eingaben vorliegen, setzen wir den Wert auf null
                    row.Cells["undiluted"].Value = null;
                }
            }
        }

    }
}