using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Data;
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
            createRibbonDropDown();
            
            LoadDataEntries();
            
        }

        private void createRibbonDropDown()
        {
            AddNumbersToDropdown(dropDownConcentrations, 1, 100);
            AddUnitsToDropDown(dropDownUnit);
            AddModelsToDropDown(dropDownModel);
        }

        private void AddModelsToDropDown(RibbonDropDown dropDown)
        {
            var models = ModelConstants.Models;
            // Für jedes Modell ein RibbonDropDownItem erstellen und hinzufügen
            foreach (var model in models)
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = model;  // Setze das Label auf den Modellnamen
                dropDown.Items.Add(item);  // Füge das Item dem Dropdown hinzu
            }
        }

        private void AddNumbersToDropdown(RibbonDropDown dropDown, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = i.ToString();
                dropDown.Items.Add(item);
            }
        }

        private void AddUnitsToDropDown(RibbonDropDown dropDown)
        {
            var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
            item.Label = "g/L";
            dropDown.Items.Add(item);
            var item2 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
            item2.Label = "mg/dL";
            dropDown.Items.Add(item2);
        }

        public void LoadDataEntries()
        {
            if (dropDownDataSet == null) return;

            List<string> tables = DatabaseHelper.GetTableNames();
            dropDownDataSet.Items.Clear(); // Vorherige Einträge löschen

            foreach (string tableName in tables)
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = tableName;
                dropDownDataSet.Items.Add(item);
            }

            // Ersten Eintrag als Standard setzen
            if (dropDownDataSet.Items.Count > 0)
            {
                
            }
            else
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Keine Datensätze";
                dropDownDataSet.Items.Add(item);
                dropDownDataSet.Enabled = false;
            }
        }

        private void buttonOpenInputForm_Click(object sender, RibbonControlEventArgs e)
        {
            string concentration = dropDownConcentrations.SelectedItem.Label;
            string unit = dropDownUnit.SelectedItem.Label;
            var form = _formFactory.CreateForm(concentration, unit);

            form.SetRibbonReference(this);
            form.ShowDialog();
        }
    }
}
