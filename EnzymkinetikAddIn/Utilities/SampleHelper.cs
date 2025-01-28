using System.Windows.Forms;

namespace EnzymkinetikAddIn.Utilities
{
    internal class SampleHelper
    {
        /// <summary>
        /// Aktualisiert die Werte in der angegebenen Spalte mit einer fortlaufenden Nummerierung.
        /// </summary>
        /// <param name="dataGridView">Die DataGridView, die aktualisiert werden soll.</param>
        /// <param name="columnName">Der Name der Spalte, die nummeriert werden soll.</param>
        public static void UpdateRowNumbers(DataGridView dataGridView, string columnName)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].Cells[columnName].Value = i + 1;
            }
        }
    }
}
