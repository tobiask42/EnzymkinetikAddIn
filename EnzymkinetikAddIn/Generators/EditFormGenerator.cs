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
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Managers;

namespace EnzymkinetikAddIn.Generators
{
    internal class EditFormGenerator : IFormGenerator
    {
        public BaseForm GenerateForm(string entryName, string tableName)
        {
            BaseForm form = new BaseForm();
            form.setSelectedTableName(tableName.Replace(" ", "_"));
            form.setEditMode(true);

            var dataGridView = form.GetDataGridView();
            DataGridManager dataGridManager = new DataGridManager(dataGridView);
            dataGridManager.LoadTable(entryName);

            // Hauptnamen an das BaseForm übergeben!
            form.ShowcomboBoxTableName(true, tableName);

            form.SetNameText(tableName);
            return form;
        }
    }
}
