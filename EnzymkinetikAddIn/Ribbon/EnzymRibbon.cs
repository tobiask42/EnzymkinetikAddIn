using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Exports;
using EnzymkinetikAddIn.Factories;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;
using Microsoft.Office.Tools.Ribbon;

namespace EnzymkinetikAddIn.Ribbon
{
    public partial class EnzymRibbon
    {
        private FormFactory _formFactory;
        private ModelFactory _modelFactory;

        // Konstanten für wiederverwendbare Strings
        private const string NoDataSetsMessage = "Keine Datensätze";
        private const string PleaseSelectTableMessage = "Bitte eine Tabelle auswählen.";
        private const string GramPerLiter = "g/L";
        private const string MgPerDLiter = "mg/dL";


        private void EnzymRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            _formFactory = new FormFactory();
            _modelFactory = new ModelFactory();
            createRibbonDropDown();
            bool debug = true;
            string databaseName = "enzymkinetik.db";
            DatabaseHelper.InitializeDatabaseConnection(debug, databaseName);
            LoadDataEntries();
            
        }

        private void createRibbonDropDown()
        {
            AddNumbersToDropdown(dropDownConcentrations, 1, 100);
            AddUnitsToDropDown(dropDownUnit);
            AddModelsToDropDown(dropDownModel);
        }

        // Gemeinsame Methode zur Erstellung von Dropdown-Items
        private void AddItemsToDropDown(RibbonDropDown dropDown, IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                var ribbonItem = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                ribbonItem.Label = item;
                dropDown.Items.Add(ribbonItem);
            }
        }

        private void AddModelsToDropDown(RibbonDropDown dropDown)
        {
            AddItemsToDropDown(dropDown, ModelConstants.Models);
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
            item.Label = GramPerLiter;
            dropDown.Items.Add(item);
            var item2 = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
            item2.Label = MgPerDLiter;
            dropDown.Items.Add(item2);
        }

        public void LoadDataEntries()
        {
            
            if (dropDownDataSet == null) return;

            List<string> tables = DatabaseHelper.GetTableNames();
            dropDownDataSet.Items.Clear(); // Vorherige Einträge löschen


            //buttonEditData.Enabled = true; //implement later
            if (tables.Count() > 0)
            {
                foreach (string tableName in tables)
                {
                    var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                    item.Label = tableName.Replace("_", " ");
                    dropDownDataSet.Items.Add(item);
                }
                if (tables.Count() == 1)
                {
                    dropDownDataSet.Enabled = false;
                    buttonEditData.Enabled = true;
                }
                else
                {
                    dropDownDataSet.Enabled = true;
                    buttonEditData.Enabled = true;
                }
            }
            else
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = NoDataSetsMessage;
                dropDownDataSet.Items.Add(item);
                dropDownDataSet.Enabled = false;
                buttonEditData.Enabled = false;
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

        private void buttonEditData_Click(object sender, RibbonControlEventArgs e)
        {
            if (dropDownDataSet.SelectedItem == null)
            {
                MessageBox.Show(PleaseSelectTableMessage, "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = dropDownDataSet.SelectedItem.Label;
            var form = _formFactory.CreateEditForm(tableName);

            form.SetRibbonReference(this);
            form.ShowDialog();
        }

        private void buttonGenerateResult_Click(object sender, RibbonControlEventArgs e)
        {
            string tableName = dropDownDataSet.SelectedItem.Label;
            string modelName = dropDownModel.SelectedItem.Label;

            try
            {
                var model = _modelFactory.GenerateModel(modelName);
                List<List<DataTable>> result = model.CalculateResult(tableName);

                ExcelExporter.ExportToExcel(result);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Fehler: " + ex.Message);
            }
        }
    }
}
