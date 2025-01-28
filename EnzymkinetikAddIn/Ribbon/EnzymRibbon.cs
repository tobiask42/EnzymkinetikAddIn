using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnzymkinetikAddIn.Factories;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Models;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Ribbon;

namespace EnzymkinetikAddIn.Ribbon
{
    public partial class EnzymRibbon
    {
        private FormFactory _formFactory;

        private void EnzymRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            _formFactory = new FormFactory();

            foreach (var model_name in _formFactory.GetAvailableModels())
            {
                AddDropDownItem(model_name);
            }
        }

        private void AddDropDownItem(string model_name)
        {
            RibbonDropDownItem item = Factory.CreateRibbonDropDownItem();
            item.Label = model_name;
            dropDownModelSelection.Items.Add(item);
        }


        private void dropDownModelSelection_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            // TODO: implement optional UI changes
        }

        private void buttonAddCalibration_Click(object sender, RibbonControlEventArgs e)
        {
            var form = _formFactory.CreateForm("Kalibriergerade");
            form.ShowDialog();
        }

        private void buttonModelChoice_Click(object sender, RibbonControlEventArgs e)
        {
            string item = dropDownModelSelection.SelectedItem.Label;
            var form = _formFactory.CreateForm(item);
            form.ShowDialog();

        }
    }
}
