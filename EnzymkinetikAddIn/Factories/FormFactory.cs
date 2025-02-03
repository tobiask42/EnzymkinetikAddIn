using System;
using System.Collections.Generic;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Generators;
using System.Linq;
using EnzymkinetikAddIn.Constants;

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

    }
}
