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
using EnzymkinetikAddIn.Utilities;
using System.Linq.Expressions;

namespace EnzymkinetikAddIn.Factories
{
    public class FormFactory
    {
        //private readonly DataSetGenerator _dataSetGenerator;

        public FormFactory()
        {
            //_dataSetGenerator = new DataSetGenerator();
        }

        public BaseForm CreateForm(string concentration, string unit)
        {
            DataSetGenerator _dataSetGenerator = new DataSetGenerator();
            return _dataSetGenerator.GenerateForm(concentration, unit);
        }

        public BaseForm CreateEditForm(string tableName)
        {
            EditFormGenerator _editFormGenerator = new EditFormGenerator();
            return _editFormGenerator.GenerateForm(tableName);
        }
    }
}
