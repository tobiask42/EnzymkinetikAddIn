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
        public List<DataGridView> GetDataGridViews(List<DataTable> tables)
        {
            List<DataGridView> result = new List<DataGridView>();
            foreach (DataTable table in tables)
            {

                DataGridView dataGridView = LoadTable(table);
                result.Add(dataGridView);
            }
            return result;
        }

        public DataGridView LoadTable(DataTable tableData)
        {
            DataGridView dataGridView = new DataGridView();
            ColumnManager  columnManager = new ColumnManager(dataGridView);

            List<DataColumn> commentColumns = new List<DataColumn>();
            columnManager.InitializeSampleColumn();
            foreach (DataColumn column in tableData.Columns)
            {
                string columnLabel = column.ColumnName;
                if (columnLabel.Contains("Zeit"))
                {
                    string timeunit = columnLabel.ElementAt(6) == 'm' ? "min" : "h";
                    columnManager.InitializeTimeColumn(timeunit);
                    WriteToGrid(column, tableData, "time", dataGridView);
                }
                else if (columnLabel.Contains("Verdünnung"))
                {
                    string number = columnLabel.ElementAt(2).ToString();
                    string dataGridColumnName = "dilution_" + number;
                    columnManager.InitializeIntCol(dataGridColumnName, "c_" + number + "\nVerdünnung");
                    WriteToGrid(column, tableData, dataGridColumnName, dataGridView);
                }
                else if (columnLabel.Contains("Messwert"))
                {
                    string sampleunit = columnLabel.Contains("g_L") ? "g/L" : "mg/dL";
                    char c = columnLabel.ElementAt(2);
                    char num = columnLabel.ElementAt(13);
                    string dataGridColumnName = num == '1' ? $"reading_1_{c}" : $"reading_2_{c}";
                    string dataGridColumnLabel = $"c_{c}\nMesswert {num}\n{sampleunit}";

                    columnManager.InitializeDoubleCol(dataGridColumnName, dataGridColumnLabel);
                    WriteToGrid(column, tableData, dataGridColumnName, dataGridView);
                }
                else
                {
                    commentColumns.Add(column);
                }
            }

            columnManager.InitializeBaseCol("comment", "Kommentar");
            foreach (var column in commentColumns)
            {
                WriteToGrid(column, tableData, "comment", dataGridView);
            }
            return dataGridView;
        }

        private void WriteToGrid(DataColumn column, DataTable tableData, string dataGridColumnName, DataGridView dataGridView)
        {
            while (dataGridView.Rows.Count <= tableData.Rows.Count)
            {
                dataGridView.Rows.Add();
            }

            for (int i = 0; i < tableData.Rows.Count; i++)
            {
                dataGridView.Rows[i].Cells[dataGridColumnName].Value = tableData.Rows[i][column];
            }
        }
    }
}
