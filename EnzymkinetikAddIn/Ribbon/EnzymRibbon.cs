using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Exports;
using EnzymkinetikAddIn.Factories;
using EnzymkinetikAddIn.Forms;
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

            List<string> entries = DatabaseHelper.GetEntryNames();
            dropDownDataSet.Items.Clear();

            if (entries.Count > 0)
            {
                foreach (string entry in entries)
                {
                    var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                    item.Label = entry;
                    dropDownDataSet.Items.Add(item);
                }
                dropDownDataSet.Enabled = true;
                buttonEditData.Enabled = true;
            }
            else
            {
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = "Keine Einträge vorhanden";
                dropDownDataSet.Items.Add(item);
                dropDownDataSet.Enabled = false;
                buttonEditData.Enabled = false;
            }
        }


        private void buttonOpenInputForm_Click(object sender, RibbonControlEventArgs e)
        {
            var form = new DualInputForm("Neue Dateneingabe", "Bitte Datensatznamen eingeben", "Bitte ersten Tabellennamen angeben");
            if (form.ShowDialog() == DialogResult.OK)
            {
                string entryName = form.EntryName;
                string tableName = form.TableName;

                if (string.IsNullOrWhiteSpace(entryName) || string.IsNullOrWhiteSpace(tableName))
                {
                    MessageBox.Show("Kein Name eingegeben. Abbruch.", "Abbruch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string concentration = dropDownConcentrations.SelectedItem.Label;
                string unit = dropDownUnit.SelectedItem.Label;
                entryName = checkForDuplicates(entryName);
                var inputForm = _formFactory.CreateForm(concentration, unit);
                inputForm.SetEntryName(entryName);
                inputForm.SetTableNames(new List<string>() { tableName });
                inputForm.AddCurrentDataGridViewToTables();
                inputForm.SetRibbonReference(this);
                inputForm.ShowDialog();

            }


        }
        private string checkForDuplicates(string input)
        {
            string unique_string = input;
            int counter = 1;

            // Überprüfe, ob der input bereits in den Items des dropDownDataSet enthalten ist
            while (dropDownDataSet.Items.Cast<RibbonDropDownItem>().Any(item => item.Label == unique_string))
            {
                // Füge eine Zahl hinzu, um den Wert einzigartig zu machen
                unique_string = input + "_" + counter.ToString();
                counter++;
            }
            return unique_string;
        }

        private void buttonEditData_Click(object sender, RibbonControlEventArgs e)
        {
            if (dropDownDataSet.SelectedItem == null) return;

            string selectedEntry = dropDownDataSet.SelectedItem.Label; // Hauptname holen
            List<DataTable> tables = DatabaseHelper.LoadTablesForEntry(selectedEntry);
            FormFactory factory = new FormFactory();
            BaseForm editForm = factory.CreateEditForm(selectedEntry, tables);
            editForm.SetRibbonReference(this);
            editForm.Show();
        }


        private void buttonGenerateResult_Click(object sender, RibbonControlEventArgs e)
        {
            string entryName = dropDownDataSet.SelectedItem.Label;
            string modelName = dropDownModel.SelectedItem.Label;

            try
            {
                var model = _modelFactory.GenerateModel(modelName);
                Dictionary<string, DataTable> result = model.CalculateResult(entryName);

                ExcelExporter.ExportToExcel(result);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Fehler: " + ex.Message);
            }
        }

        private void buttonAddTable_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
