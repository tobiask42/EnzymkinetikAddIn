﻿using System;
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
            form.setEditMode(true);
            List<DataGridView> dataGridViews = dataGridManager.GetDataGridViews(tables);
            form.SetTableList(dataGridViews);
            return form;
        }
    }
}
