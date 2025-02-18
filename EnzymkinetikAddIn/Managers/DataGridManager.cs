using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Utilities;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Managers
{
    internal class DataGridManager
    {
        private readonly DataGridView _dataGridView;
        private readonly ColumnManager _columnManager;

        public DataGridManager(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            _columnManager = new ColumnManager(_dataGridView);
        }

        public void LoadTable(string tableName)
        {
            DataTable tableData = DatabaseHelper.LoadTable(tableName);
            _dataGridView.Rows.Clear();
            _dataGridView.Columns.Clear();

            List<DataColumn> commentColumns = new List<DataColumn>();
            _columnManager.InitializeSampleColumn();
            foreach (DataColumn column in tableData.Columns)
            {
                string columnLabel = column.ColumnName;
                if (columnLabel.Contains("Zeit"))
                {
                    string timeunit = columnLabel.ElementAt(6) == 'm' ? "min" : "h";
                    _columnManager.InitializeTimeColumn(timeunit);
                    WriteToGrid(column, tableData, "time");
                }
                else if (columnLabel.Contains("Verdünnung"))
                {
                    string number = columnLabel.ElementAt(2).ToString();
                    string dataGridColumnName = "dilution_" + number;
                    _columnManager.InitializeIntCol(dataGridColumnName, "c_" + number + "\nVerdünnung");
                    WriteToGrid(column, tableData, dataGridColumnName);
                }
                else if (columnLabel.Contains("Messwert"))
                {
                    string sampleunit = columnLabel.Contains("g_L") ? "g/L" : "mg/dL";
                    char c = columnLabel.ElementAt(2);
                    char num = columnLabel.ElementAt(13);
                    string dataGridColumnName = num == '1' ? $"reading_1_{c}" : $"reading_2_{c}";
                    string dataGridColumnLabel = $"c_{c}\nMesswert {num}\n{sampleunit}";

                    _columnManager.InitializeDoubleCol(dataGridColumnName, dataGridColumnLabel);
                    WriteToGrid(column, tableData, dataGridColumnName);
                }
                else
                {
                    commentColumns.Add(column);
                }
            }

            _columnManager.InitializeBaseCol("comment", "Kommentar");
            foreach (var column in commentColumns)
            {
                WriteToGrid(column, tableData, "comment");
            }
        }

        private void WriteToGrid(DataColumn column, DataTable tableData, string dataGridColumnName)
        {
            while (_dataGridView.Rows.Count <= tableData.Rows.Count)
            {
                _dataGridView.Rows.Add();
            }

            for (int i = 0; i < tableData.Rows.Count; i++)
            {
                _dataGridView.Rows[i].Cells[dataGridColumnName].Value = tableData.Rows[i][column];
            }
        }
    }
}
