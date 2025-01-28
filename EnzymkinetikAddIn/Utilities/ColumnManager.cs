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

            _grid.CellFormatting += FormatTimeCell;
            _grid.CellParsing += ParseTimeCell;
            return this;
        }

        private void ParseTimeCell(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == _grid.Columns["time"].Index && e.Value is string input)
            {
                if (double.TryParse(input.Replace(".", ","), out double parsedValue))
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

        private void FormatTimeCell(object sender, DataGridViewCellFormattingEventArgs e)
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

            // Anhängen des EventHandlers für Eingabevalidierung
            DataGridViewTextBoxHandler.AttachTextBoxEventHandler(
                _grid,
                name,
                (sender, e) => e.Handled = !ValidationHelper.IsValidIntInput(e.KeyChar)
            );

            return this;
        }

        public ColumnManager InitializeDoubleCol(string name, string header, bool readOnly = false)
        {
            DataGridViewTextBoxColumn doubleColumn = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                ValueType = typeof(double),
                ReadOnly = readOnly,
            };
            _grid.Columns.Add(doubleColumn);

            // Binde das Event "EditingControlShowing", um den Event-Handler für die Double-Validierung hinzuzufügen
            _grid.EditingControlShowing += (sender, e) =>
            {
                if (_grid.CurrentCell?.OwningColumn?.Name == name && e.Control is TextBox textBox)
                {
                    // Entferne alte Event-Handler, um doppelte Registrierungen zu vermeiden
                    textBox.KeyPress -= DoubleColumnKeyPressHandler;

                    // Füge neuen Event-Handler hinzu
                    textBox.KeyPress += DoubleColumnKeyPressHandler;
                }
            };

            return this;
        }

        // Event-Handler für die Double-Validierung
        private void DoubleColumnKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Validierung der Eingabe
                e.Handled = !ValidationHelper.IsValidDoubleInput(textBox, e.KeyChar);
            }
        }
    }
}
