using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Factories;
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
                item.Label = "Keine Datensätze";
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
                MessageBox.Show("Bitte eine Tabelle auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = dropDownDataSet.SelectedItem.Label;
            var form = _formFactory.CreateEditForm(tableName);

            form.SetRibbonReference(this);
            form.ShowDialog();
        }
    }
}
