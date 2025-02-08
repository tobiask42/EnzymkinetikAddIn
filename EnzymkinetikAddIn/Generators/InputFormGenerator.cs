using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Generators
{
    internal class InputFormGenerator : IFormGenerator
    {
        public BaseForm GenerateForm(string concentration, string unit)
        {
            // Erstellt ein neues Formular
            BaseForm form = new BaseForm();

            // Zugriff auf das DataGridView des Formulars
            var dataGridView = form.GetDataGridView();

            ConfigureColumns(dataGridView, concentration, unit);

            form.showDeleteButton(false);
            // Gibt das konfigurierte Formular zurück
            return form;
        }

        public void ConfigureColumns(DataGridView dataGridView, string concentration, string unit)
        {
            int num_concentrations = 1;
            try
            {
                num_concentrations = Int32.Parse(concentration);
            }
            catch
            {
            }
            var columnManager = new ColumnManager(dataGridView);
            columnManager.InitializeTimeColumn();
            for (int i = 1; i <= num_concentrations; i++)
            {
                columnManager.InitializeIntCol("dilution_" + i, "c_" + i + "\nVerdünnung");
                columnManager.InitializeDoubleCol("reading_1_" + i, "c_"+ i +"\nMesswert 1\n" + unit);
                columnManager.InitializeDoubleCol("reading_2_" + i, "c_" + i + "\nMesswert 2\n" + unit);
            }
            columnManager.InitializeBaseCol("comment", "Kommentar");
        }
    }
}
