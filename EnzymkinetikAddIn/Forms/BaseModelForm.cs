using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Forms
{
    /// <summary>
    /// Generisches Formular für die Eingabe von Daten. Automatische Probennummerierung und Zeitangabe.
    /// </summary>
    public partial class BaseModelForm : Form
    {
        private string CurrentTimeUnit = "h"; // Standard: Stunden

        private ComboBoxManager _comboBoxManager;
        public BaseModelForm()
        {
            InitializeComponent();
            DataGridViewTextBoxColumn sampleColum = new DataGridViewTextBoxColumn
            {
                Name = "sample",
                HeaderText = "Probe",
                ValueType = typeof(int),
                ReadOnly = true,
            };
            dataGridViewInputData.Columns.Add(sampleColum);

            // Initialisiere den ComboBoxManager
            _comboBoxManager = new ComboBoxManager(comboBoxTimeUnit, labelTimeUnit);
            UpdateComboBoxVisibility();

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
            comboBoxTimeUnit.SelectedIndex = 0; // Setzt Standardwert auf "h"


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
                        cell.Value = TimeConverter.ConvertTime(timeValue, CurrentTimeUnit, newUnit);
                    }
                }
            }

            // Aktualisiere die aktuelle Einheit
            CurrentTimeUnit = newUnit;

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

        /// <summary>
        /// Formatiert die Werte in der "time"-Spalte.
        /// </summary>
        private void FormatTimeCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewInputData.Columns["time"].Index && e.Value is double)
            {
                e.Value = ((double)e.Value).ToString("N", CultureInfo.CurrentCulture);
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Parst Eingaben in der "time"-Spalte.
        /// </summary>
        private void ParseTimeCell(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewInputData.Columns["time"].Index && e.Value is string)
            {
                string input = e.Value.ToString().Replace(".", ","); // Punkt zu Komma
                if (double.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out double parsedValue))
                {
                    e.Value = parsedValue;
                    e.ParsingApplied = true;
                }
                else
                {
                    e.ParsingApplied = false;
                }
            }
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

        public void setDataGridView(DataGridView dataGridViewInputData)
        {
            this.dataGridViewInputData = dataGridViewInputData;
        }
    }
}
