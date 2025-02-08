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

        public BaseForm CreateForm(string concentration, string unit)
        {
            InputFormGenerator _dataSetGenerator = new InputFormGenerator();
            return _dataSetGenerator.GenerateForm(concentration, unit);
        }

        public BaseForm CreateEditForm(string tableName)
        {
            EditFormGenerator _editFormGenerator = new EditFormGenerator();
            return _editFormGenerator.GenerateForm("", tableName);
        }
    }
}
