using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Utilities
{
    internal class ComboBoxManager
    {
        private readonly ComboBox _comboBox;
        private readonly Label _label;

        public string Unit { get; private set; } = "h"; // Standard-Zeiteinheit

        public ComboBoxManager(ComboBox comboBox, Label label)
        {
            _comboBox = comboBox;
            _label = label;

            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            _comboBox.Items.AddRange(new string[] { "h", "min", "s" });
            _comboBox.SelectedIndex = 0; // Setzt den Standardwert "h"
            _comboBox.SelectedIndexChanged += ComboBoxTimeUnit_SelectedIndexChanged;

            // Standardmäßig sichtbar
            Show();
        }

        public void UpdateVisibility(bool hasTimeColumn)
        {
            if (hasTimeColumn)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            _comboBox.Visible = true;
            _label.Visible = true;
        }

        private void Hide()
        {
            _comboBox.Visible = false;
            _label.Visible = false;
        }

        public event EventHandler UnitChanged;

        private void ComboBoxTimeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_comboBox.SelectedItem is string newUnit && Unit != newUnit)
            {
                Unit = newUnit;
                UnitChanged?.Invoke(this, EventArgs.Empty);
            }
        }

    }
}