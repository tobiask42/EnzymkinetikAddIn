using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Utilities
{
    internal class DataGridViewUtility
    {
        /// <summary>
        /// Erstellt eine neue konfigurierte DataGridView basierend auf den gegebenen Parametern.
        /// </summary>
        public static DataGridView CreateConfiguredDataGridView(string concentration, string unit)
        {
            DataGridView dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            };

            ConfigureColumns(dataGridView, concentration, unit);
            return dataGridView;
        }

        /// <summary>
        /// Konfiguriert die Spalten einer bestehenden DataGridView.
        /// </summary>
        public static void ConfigureColumns(DataGridView dataGridView, string concentration, string unit)
        {
            int numConcentrations = 1;
            int.TryParse(concentration, out numConcentrations);

            var columnManager = new ColumnManager(dataGridView);
            columnManager.InitializeSampleColumn();
            columnManager.InitializeTimeColumn("h");

            for (int i = 1; i <= numConcentrations; i++)
            {
                columnManager.InitializeIntCol("dilution_" + i, "c_" + i + "\nVerdünnung");
                columnManager.InitializeDoubleCol("reading_1_" + i, "c_" + i + "\nMesswert 1\n" + unit);
                columnManager.InitializeDoubleCol("reading_2_" + i, "c_" + i + "\nMesswert 2\n" + unit);
            }

            columnManager.InitializeBaseCol("comment", "Kommentar");
        }
    }
}
