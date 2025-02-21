using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Forms
{
    internal class DualInputForm : Form
    {
        public string EntryName => textBoxEntryName.Text;
        public string TableName => textBoxTableName.Text;

        public DualInputForm(string title, string label1, string label2)
        {
            InitializeComponent();
            this.Text = title;

            labelEntryName.Text = label1;
            labelTableName.Text = label2;

            // KeyPress-Events anhängen
            textBoxEntryName.KeyPress += nameTextBox_KeyPress;
            textBoxTableName.KeyPress += nameTextBox_KeyPress;
        }

        private void InitializeComponent()
        {
            this.textBoxEntryName = new TextBox { Location = new Point(20, 40), Width = 200 };
            this.textBoxTableName = new TextBox { Location = new Point(20, 100), Width = 200 };
            this.labelEntryName = new Label { Text = "Datensatzname:", Location = new Point(20, 20) };
            this.labelTableName = new Label { Text = "Tabellenname:", Location = new Point(20, 80) };
            this.buttonOK = new Button { Text = "OK", Location = new Point(20, 160), DialogResult = DialogResult.OK };
            this.buttonCancel = new Button { Text = "Abbrechen", Location = new Point(140, 160), DialogResult = DialogResult.Cancel };

            this.Controls.Add(labelEntryName);
            this.Controls.Add(textBoxEntryName);
            this.Controls.Add(labelTableName);
            this.Controls.Add(textBoxTableName);
            this.Controls.Add(buttonOK);
            this.Controls.Add(buttonCancel);

            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(250, 220);
        }

        private TextBox textBoxEntryName;
        private TextBox textBoxTableName;
        private Label labelEntryName;
        private Label labelTableName;
        private Button buttonOK;
        private Button buttonCancel;

        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;

            // Verhindert, dass der Name mit einer Zahl oder einem Leerzeichen beginnt
            if (currentTextBox.TextLength == 0 && (char.IsDigit(e.KeyChar) || e.KeyChar == ' '))
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
    }
}
