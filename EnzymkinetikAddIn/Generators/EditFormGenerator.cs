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
    internal class EditFormGenerator
    {
        public BaseForm GenerateForm(string entryName, List<DataTable> tables)
        {
            DataGridManager dataGridManager = new DataGridManager();
            BaseForm form = new BaseForm();
            form.Name = entryName;
            DataGridView baseDataGridView = form.GetDataGridView();
            form.setEditMode(true);
            MessageBox.Show("tables in GenerateForm: " + tables.Count());
            List<DataGridView> dataGridViews = dataGridManager.GetDataGridViews(tables);
            MessageBox.Show("datagridviews in generateform: " + dataGridViews.Count());
            form.SetTableList(dataGridViews);
            List<string> tablenames = DatabaseHelper.GetTableNamesByEntryName(entryName);
            form.SetTableNames(tablenames);
            return form;
        }
    }
}
