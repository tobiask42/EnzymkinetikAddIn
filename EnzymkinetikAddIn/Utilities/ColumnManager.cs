using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Utilities
{
    internal class ColumnManager
    {
        private readonly DataGridView _grid;
        private string _unit = "h";

        public ColumnManager(DataGridView grid)
        {
            _grid = grid;
            _grid.EditingControlShowing += Grid_EditingControlShowing;
        }


        public ColumnManager InitializeTimeColumn()
        {
            DataGridViewTextBoxColumn timeColumn = new DataGridViewTextBoxColumn
            {
                Name = "time",
                HeaderText = "Zeit (" + _unit + ")",
                ValueType = typeof(double),
            };
            _grid.Columns.Add(timeColumn);

            // Eingabevalidierung für die Zeitspalte hinzufügen
            DataGridViewTextBoxHandler.AttachTextBoxEventHandler(
                _grid,
                "time",
                (sender, e) =>
                {
                    if (sender is TextBox textBox)
                        e.Handled = !ValidationHelper.IsValidDoubleInput(textBox, e.KeyChar);
                }
            );

            _grid.CellFormatting += FormatDoubleCell;
            _grid.CellParsing += ParseDoubleCell;
            return this;
        }

        private void ParseDoubleCell(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.Value is string input)
            {
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out double parsedValue))
                {
                    e.Value = parsedValue;
                    e.ParsingApplied = true;
                }
                else if (double.TryParse(input.Replace(".", ","), NumberStyles.Float, CultureInfo.GetCultureInfo("de-DE"), out parsedValue))
                {
                    e.Value = parsedValue;
                    e.ParsingApplied = true;
                }
                else if (double.TryParse(input.Replace(",", "."), NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue))
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


        private void FormatDoubleCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == _grid.Columns["time"].Index && e.Value is double value)
            {
                e.Value = value.ToString("N", CultureInfo.CurrentCulture);
                e.FormattingApplied = true;
            }
        }

        public ColumnManager InitializeBaseCol(string name, string header, bool readOnly = false)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                ReadOnly = readOnly,
            };
            _grid.Columns.Add(column);
            return this;
        }

        public ColumnManager InitializeIntCol(string name, string header, bool readOnly = false)
        {
            DataGridViewTextBoxColumn intColumn = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                ValueType = typeof(int),
                ReadOnly = readOnly,
            };
            _grid.Columns.Add(intColumn);

            return this;
        }

        private void IntColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !ValidationHelper.IsValidIntInput(e.KeyChar);
        }


        public ColumnManager InitializeDoubleCol(string name, string header, bool readOnly = false)
        {
            DataGridViewTextBoxColumn doubleColumn = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                ValueType = typeof(double),
                ReadOnly = readOnly,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "F2" // Format für 2 Nachkommastellen
                }
            };
            _grid.Columns.Add(doubleColumn);

            // Einheitliche Validierung für alle Double-Spalten
            _grid.CellFormatting += FormatDoubleCell;
            _grid.CellParsing += ParseDoubleCell;

            return this;
        }


        // Event-Handler für die Bearbeitung der Spalten
        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                // Alle bisherigen Event-Handler entfernen (Verhindert Mehrfachbindung)
                textBox.KeyPress -= IntColumn_KeyPress;
                textBox.KeyPress -= DoubleColumnKeyPressHandler;

                // Bestimme den erwarteten Datentyp der aktuellen Spalte
                var valueType = _grid.CurrentCell?.OwningColumn?.ValueType;

                if (valueType == typeof(int))
                {
                    textBox.KeyPress += IntColumn_KeyPress; // Nur Ganzzahlen erlaubt
                }
                else if (valueType == typeof(double))
                {
                    textBox.KeyPress += DoubleColumnKeyPressHandler; // Nur gültige Double-Werte
                }
            }
        }



        // Event-Handler für die Double-Validierung
        private void DoubleColumnKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Prüfen, ob die aktuelle Zelle eine Double-Spalte ist
                if (_grid.CurrentCell?.OwningColumn?.ValueType == typeof(double))
                {
                    // Einheitliche Validierung mit Punkt- und Komma-Unterstützung
                    e.Handled = !ValidationHelper.IsValidDoubleInput(textBox, e.KeyChar);
                }
            }
        }

        public void SetTimeUnit(string _unit)
        {
            this._unit = _unit;
        }
    }
}
