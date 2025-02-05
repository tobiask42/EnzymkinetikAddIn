using System;
using System.Collections.Generic;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Generators;
using System.Linq;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Data;
using System.Windows.Forms;
using System.Data;

namespace EnzymkinetikAddIn.Factories
{
    public class FormFactory
    {
        private readonly DataSetGenerator _dataSetGenerator;

        public FormFactory()
        {
            _dataSetGenerator = new DataSetGenerator();
        }

        public BaseForm CreateForm(string concentration, string unit)
        {
            return _dataSetGenerator.GenerateForm(concentration, unit);
        }

        public BaseForm CreateEditForm(string tableName)
        {
            // Lade die Daten aus der Datenbank
            DataTable tableData = DatabaseHelper.LoadTable(tableName);

            // Erstelle das Formular
            BaseForm form = new BaseForm();

            // Zugriff auf das DataGridView des Formulars
            var dataGridView = form.GetDataGridView();

            // Falls Daten vorhanden sind ins Grid setzen
            if (tableData.Rows.Count > 0)
            {
                dataGridView.DataSource = tableData;
            }
            else
            {
                MessageBox.Show("Die ausgewählte Tabelle enthält keine Daten.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return form;
        }


    }
}
