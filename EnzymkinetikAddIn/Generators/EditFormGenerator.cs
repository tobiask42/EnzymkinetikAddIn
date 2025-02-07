using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Data;
using System.Windows.Forms;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Generators
{
    internal class EditFormGenerator
    {
        public BaseForm GenerateForm(string tableName)
        {
            // Lade die Daten aus der Datenbank
            DataTable tableData = DatabaseHelper.LoadTable(tableName);

            // Erstelle das Formular
            BaseForm form = new BaseForm();

            // Zugriff auf das DataGridView des Formulars
            var dataGridView = form.GetDataGridView();

            // Initialisiere die Spalten über den ColumnManager
            ColumnManager columnManager = new ColumnManager(dataGridView);

            List<DataColumn> commentColumns = new List<DataColumn>(); // Speichert Kommentar-Spalten zur späteren Verarbeitung

            string currentTimeUnit = "";
            // Über die Spalten des DataTables iterieren
            foreach (DataColumn column in tableData.Columns)
            {
                string columnLabel = column.ColumnName;
                if (columnLabel.Contains("Zeit"))
                {
                    string timeunit = Char.ToString(columnLabel.ElementAt(6));
                    if (timeunit.Equals("m"))
                    {
                        timeunit = "min";
                    }
                    currentTimeUnit = timeunit;
                    columnManager.SetTimeUnit(timeunit);
                    columnManager.InitializeTimeColumn();
                    WriteToGrid(column, tableData, dataGridView, "time");
                }
                else if (columnLabel.Contains("Verdünnung"))
                {
                    string number = Char.ToString(columnLabel.ElementAt(2));
                    string dataGridColumnName = "dilution_" + number;
                    columnManager.InitializeIntCol(dataGridColumnName, "c_" + number + "\nVerdünnung");
                    WriteToGrid(column, tableData, dataGridView, dataGridColumnName);
                }
                else if (columnLabel.Contains("Messwert"))
                {
                    string sampleunit = columnLabel.Contains("g_L") ? "g/L" : "mg/dL";
                    char c = columnLabel.ElementAt(2);
                    char num = columnLabel.ElementAt(13);
                    string dataGridColumnName = num == '1' ? $"reading_1_{c}" : $"reading_2_{c}";
                    string dataGridColumnLabel = $"c_{c}\nMesswert {num}\n{sampleunit}";

                    columnManager.InitializeDoubleCol(dataGridColumnName, dataGridColumnLabel);
                    WriteToGrid(column, tableData, dataGridView, dataGridColumnName);
                }
                else
                {
                    commentColumns.Add(column); // Kommentar-Spalten zwischenspeichern
                }
            }

            // Kommentar-Spalte zuletzt hinzufügen
            columnManager.InitializeBaseCol("comment", "Kommentar");
            foreach (var column in commentColumns)
            {
                WriteToGrid(column, tableData, dataGridView, "comment");
            }
            form.SetCurrentTimeUnit(currentTimeUnit);
            return form;
        }


        private void WriteToGrid(DataColumn column, DataTable tableData, DataGridView dataGridView, string dataGridColumnName)
        {
            // Stelle sicher, dass das DataGridView genügend Zeilen hat und füge weitere Eingabezeile hinzu
            while (dataGridView.Rows.Count <= tableData.Rows.Count)
            {
                dataGridView.Rows.Add();
            }

            // Schreibe die Werte aus der DataTable in die entsprechende Spalte der DataGridView
            for (int i = 0; i < tableData.Rows.Count; i++)
            {
                dataGridView.Rows[i].Cells[dataGridColumnName].Value = tableData.Rows[i][column];
            }
        }
    }
}
