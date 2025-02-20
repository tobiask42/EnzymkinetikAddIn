using System;
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
using EnzymkinetikAddIn.Managers;
using EnzymkinetikAddIn.Ribbon;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Forms
{
    /// <summary>
    /// Generisches Formular für die Eingabe von Daten. Automatische Probennummerierung und Zeitangabe.
    /// </summary>
    public partial class BaseForm : Form
    {
        private DataGridManager _dataGridManager;
        private string currentTimeUnit = "";
        private EnzymRibbon _ribbon;
        private string selectedTableName = "";
        private bool editMode = false;
        private string _entryName;
        private List<DataGridView> _tableList = new List<DataGridView>();
        private int _currentTableIndex = 0;
        private string _selectedUnit;
        private string _selectedConcentration;
        private readonly ComboBoxManager _comboBoxManager;
        public BaseForm()
        {
            InitializeComponent();
            _comboBoxManager = new ComboBoxManager(comboBoxTimeUnit, labelTimeUnit);
            UpdateComboBoxVisibility();
            InitializeDropdowns();
            _selectedConcentration = comboBoxConcentration.Text;
            _selectedUnit = comboBoxUnit.Text;

        }

        public void ShowcomboBoxTableName(bool visible, string entryName)
        {
            if (visible)
            {
                InitializeEntryNameComboBox(entryName);
            }
                if (Controls.ContainsKey("comboBoxTableName"))
            {
                Controls["comboBoxTableName"].Visible = visible;
            }
            if (Controls.ContainsKey("labelTables"))
            {
                Controls["labelTables"].Visible = visible;
            }
        }

        public void ShowcomboBoxTableName(bool visible)
        {
            if (Controls.ContainsKey("comboBoxTableName"))
            {
                Controls["comboBoxTableName"].Visible=visible;
            }
            if (Controls.ContainsKey("labelTables"))
            {
                Controls["labelTables"].Visible = visible;
            }
        }

        private void InitializeDropdowns()
        {
            comboBoxUnit.Items.AddRange(new string[] { "g/L", "mg/L"});
            comboBoxUnit.SelectedIndex = 0;
            comboBoxUnit.SelectedIndexChanged += (s, e) => _selectedUnit = comboBoxUnit.SelectedItem.ToString();

            comboBoxConcentration.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            comboBoxConcentration.SelectedIndex = 0;
            comboBoxConcentration.SelectedIndexChanged += (s, e) => _selectedConcentration = comboBoxConcentration.SelectedItem.ToString();
        }

        private void InitializeEntryNameComboBox(string entryName)
        {
            comboBoxTableName.Items.Clear();
            List<string> tableNames = DatabaseHelper.GetTablesForEntry(entryName);

            foreach (string table in tableNames)
            {
                comboBoxTableName.Items.Add(table);
            }

            if (comboBoxTableName.Items.Count > 0)
            {
                comboBoxTableName.SelectedIndex = 0;
            }
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

        public DataGridView GetDataGridView()
        {
            return dataGridViewInputData;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Bitte Namen für die Tabelle eingeben.", "Fehlender Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return;
            }

            try
            {
                string tableName = nameTextBox.Text.Trim();
                tableName = Regex.Replace(tableName, @"\s+", "_");

                // Frage: Soll der Nutzer eine weitere Tabelle hinzufügen?
                DialogResult result = MessageBox.Show(
                    "Möchten Sie eine weitere Tabelle hinzufügen?",
                    "Neue Tabelle",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Leere Tabelle erstellen und anzeigen
                    _currentTableIndex = _tableList.Count;
                    CreateNewTable();
                    comboBoxTableName.Visible = true; // ComboBox sichtbar machen
                    UpdateTableSelection();
                    return;
                }

                // Falls der Nutzer fertig ist: Speichern aller Tabellen in die Datenbank
                SaveAllTablesToDatabase();
                _ribbon.LoadDataEntries();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Speichern: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Erstellt eine neue leere Tabelle
        private void CreateNewTable()
        {
            if (_tableList.Count < 1)
            {
                _tableList.Add(dataGridViewInputData);
            }

            var location = dataGridViewInputData.Location;
            var size = dataGridViewInputData.Size;
            var anchor = dataGridViewInputData.Anchor;
            // Neue DataGridView erstellen
            dataGridViewInputData = DataGridViewUtility.CreateConfiguredDataGridView(_selectedConcentration, _selectedUnit);

            // Gleiche Position, Größe und Anchor übernehmen
            dataGridViewInputData.Location = location;
            dataGridViewInputData.Size = size;
            dataGridViewInputData.Anchor = anchor;

            // Der Liste hinzufügen
            _tableList.Add(dataGridViewInputData);

            // In das UI einfügen
            Controls.Add(dataGridViewInputData);
            dataGridViewInputData.BringToFront();
        }




        // Aktualisiert die ComboBox für den Tabellenauswahl
        private void UpdateTableSelection()
        {
            comboBoxTableName.Items.Clear();

            for (int i = 0; i < _tableList.Count; i++)
            {
                comboBoxTableName.Items.Add($"Tabelle {i + 1}");
            }

            // Sicherstellen, dass der Index innerhalb des gültigen Bereichs liegt
            if (_tableList.Count > 0)
            {
                _currentTableIndex = Math.Min(_currentTableIndex, _tableList.Count - 1);
                comboBoxTableName.SelectedIndex = _currentTableIndex;
            }
        }


        // Wechselt zwischen gespeicherten Tabellen
        private void comboBoxTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTableName.SelectedIndex >= 0 && comboBoxTableName.SelectedIndex < _tableList.Count)
            {
                // Entferne die aktuell sichtbare Tabelle aus den Controls
                Controls.Remove(dataGridViewInputData);

                // Wechsle zur gewählten Tabelle aus der Liste
                dataGridViewInputData = _tableList[comboBoxTableName.SelectedIndex];

                // Füge sie zum UI hinzu
                Controls.Add(dataGridViewInputData);
                dataGridViewInputData.BringToFront();
            }
        }



        // Speichert alle Tabellen in die Datenbank
        private void SaveAllTablesToDatabase()
        {
            foreach (var (table, index) in _tableList.Select((t, i) => (t, i)))
            {
                string tableName = $"Tabelle_{index + 1}";
                DatabaseHelper.SaveDataGridViewToDatabase(table, _entryName, tableName);
            }

            MessageBox.Show("Alle Tabellen wurden erfolgreich gespeichert!", "Speichern erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        internal void SetEntryName(string entryName)
        {
            _entryName = entryName;
        }

        private void comboBoxConcentration_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedConcentration = comboBoxConcentration.Text;
        }

        public void SetTableList(List<DataGridView> dataGridViews)
        {
            _tableList = dataGridViews;
        }
    }
}
