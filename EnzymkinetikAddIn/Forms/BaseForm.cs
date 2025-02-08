﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Ribbon;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Forms
{
    /// <summary>
    /// Generisches Formular für die Eingabe von Daten. Automatische Probennummerierung und Zeitangabe.
    /// </summary>
    public partial class BaseForm : Form
    {
        private string currentTimeUnit = ""; // Standard: Stunden
        private EnzymRibbon _ribbon;
        private string selectedTableName = "";
        bool editMode = false;

        private readonly ComboBoxManager _comboBoxManager;
        public BaseForm()
        {
            InitializeComponent();
            InitializeSampleColumn();

            // Initialisiere den ComboBoxManager
            _comboBoxManager = new ComboBoxManager(comboBoxTimeUnit, labelTimeUnit);
            UpdateComboBoxVisibility();

        }

        private void InitializeSampleColumn()
        {
            var sampleColumn = new DataGridViewTextBoxColumn
            {
                Name = "sample",
                HeaderText = "Probe",
                ValueType = typeof(int),
                ReadOnly = true
            };
            dataGridViewInputData.Columns.Add(sampleColumn);
        }

        private void UpdateComboBoxVisibility()
        {
            if (_comboBoxManager == null)
                return; // Schutzabfrage gegen NullReferenceException

            bool hasTimeColumn = dataGridViewInputData.Columns.Contains("time");
            _comboBoxManager.UpdateVisibility(hasTimeColumn);

            if (hasTimeColumn)
            {
                DataGridViewTextBoxHandler.AttachTextBoxEventHandler(
                    dataGridViewInputData,
                    "time",
                    ValidateTimeInput
                );
            }
        }


        private void BaseModelForm_Load(object sender, EventArgs e)
        {
            _comboBoxManager.UnitChanged += (s, eventArgs) =>
            {
                UpdateTimeColumn(); // Methode zur Aktualisierung der Zeitspalte
            };

            UpdateComboBoxVisibility();
            // Setze die Auswahl explizit auf die aktuelle Zeiteinheit
            if (!string.IsNullOrEmpty(currentTimeUnit) && comboBoxTimeUnit.Items.Contains(currentTimeUnit))
            {
                comboBoxTimeUnit.SelectedItem = currentTimeUnit;
            }
            else
            {
                comboBoxTimeUnit.SelectedIndex = 0; // Fallback auf Standardwert "h"
                currentTimeUnit = comboBoxTimeUnit.SelectedItem.ToString(); // Synchronisieren
            }

            UpdateTimeColumn();

            ColumnSetup();

            SetMaximumSize();
        }

        /// <summary>
        /// Aktualisiert die Zeitspalte basierend auf der aktuellen Einheit im ComboBoxManager.
        /// </summary>
        private void UpdateTimeColumn()
        {
            if (_comboBoxManager == null) return;

            string newUnit = _comboBoxManager.Unit;

            foreach (DataGridViewRow row in dataGridViewInputData.Rows)
            {
                if (row.Cells["time"] is DataGridViewCell cell && cell.Value != null)
                {
                    if (double.TryParse(cell.Value.ToString(), out double timeValue))
                    {
                        cell.Value = TimeConverter.ConvertTime(timeValue, currentTimeUnit, newUnit);
                    }
                }
            }

            // Aktualisiere die aktuelle Einheit
            currentTimeUnit = newUnit;

            // Aktualisiere den Spaltenkopf
            if (dataGridViewInputData.Columns["time"] is DataGridViewColumn timeColumn)
            {
                timeColumn.HeaderText = $"Zeit ({newUnit})";
            }
        }


        private void ColumnSetup()
        {
            // Spaltenbreite automatisch anpassen und Sortierbarkeit deaktivieren.
            dataGridViewInputData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewInputData.AllowUserToOrderColumns = false;
            foreach (DataGridViewColumn column in dataGridViewInputData.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// Legt die maximale Größe des Formulars basierend auf der Bildschirmauflösung fest.
        /// </summary>
        private void SetMaximumSize()
        {
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            this.MaximumSize = new Size(screenWidth, screenHeight);
        }




        private void comboBoxTimeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_comboBoxManager == null) return;

            string unit = _comboBoxManager.Unit;

            foreach (DataGridViewRow row in dataGridViewInputData.Rows)
            {
                if (row.Cells["time"] is DataGridViewCell currentCell && currentCell.Value is double timeValue)
                {
                    currentCell.Value = TimeConverter.ConvertTime(timeValue, _comboBoxManager.Unit, unit);
                }
            }

            if (dataGridViewInputData.Columns["time"] is DataGridViewColumn timeColumn)
            {
                timeColumn.HeaderText = $"Zeit ({unit})";
            }
        }


        /// <summary>
        /// Benutzerdefinierte Validierung für die "time"-Spalte.
        /// </summary>
        private void ValidateTimeInput(object sender, KeyPressEventArgs e)
        {
            
        }

        private void dataGridViewInputData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SampleHelper.UpdateRowNumbers(dataGridViewInputData, "sample");
        }

        private void dataGridViewInputData_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            SampleHelper.UpdateRowNumbers(dataGridViewInputData, "sample");
        }

        public DataGridView GetDataGridView()
        {
            return dataGridViewInputData;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (editMode)
            {
                EditTable();
                _ribbon.LoadDataEntries();
                this.Close();
                return;
            }
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Bitte Namen für die Tabelle eingeben.", "Fehlender Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return;
            }

            try
            {

                string tablename = nameTextBox.Text.Trim();
                tablename = Regex.Replace(tablename, @"\s+", "_");

                bool success = DatabaseHelper.SaveDataGridViewToDatabase(dataGridViewInputData, tablename);
                tablename = DatabaseHelper.StorageName.Replace("_"," ");
                if (success)
                {
                    MessageBox.Show("Daten wurden erfolgreich unter " + tablename + " gespeichert.", "Speichern erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ribbon.LoadDataEntries();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditTable()
        {
            DatabaseHelper.UpdateTableFromDataGridView(dataGridViewInputData,selectedTableName,nameTextBox.Text);
        }

        public void SetRibbonReference(EnzymRibbon ribbon)
        {
            _ribbon = ribbon;
        }

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verhindert, dass der Name mit einer Zahl oder einem Leerzeichen beginnt
            if ((nameTextBox.TextLength == 0 && (char.IsDigit(e.KeyChar) || e.KeyChar == ' ')))
            {
                e.Handled = true;
            }
            // Erlaubt Buchstaben, Ziffern, Steuerzeichen (wie Backspace) und Leerzeichen
            else if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == ' ')
            {
                e.Handled = false;  // Zeichen zulassen
            }
            // Verhindert alle anderen Zeichen
            else
            {
                e.Handled = true;  // Zeichen blockieren
            }
        }

        public void SetCurrentTimeUnit(string timeUnit)
        {
            currentTimeUnit = timeUnit;

            if (comboBoxTimeUnit != null && comboBoxTimeUnit.Items.Contains(timeUnit))
            {
                comboBoxTimeUnit.SelectedItem = timeUnit;
            }
        }

        public void SetNameText(string name)
        {
            nameTextBox.Text = name;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(selectedTableName);
            DatabaseHelper.DeleteTable(selectedTableName);
            _ribbon.LoadDataEntries();
            this.Close();
        }

        public void showDeleteButton(bool showDelete)
        {
            deleteButton.Visible = showDelete;
        }

        public void setSelectedTableName(string tableName)
        {
            selectedTableName = tableName;
        }

        public string getSelectedTableName()
        {
            return selectedTableName;
        }

        public void setEditMode(bool editMode)
        {
            this.editMode = editMode;
        }
    }
}
