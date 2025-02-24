using System;
using System.Drawing;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Forms
{
    internal class DualInputForm : Form
    {
        public string EntryName => textBoxEntryName.Text;
        public string TableName => textBoxTableName.Text;

        public DualInputForm(string title)
        {
            InitializeComponent();
            this.Text = title;

            // KeyPress-Events anhängen
            textBoxEntryName.KeyPress += nameTextBox_KeyPress;
            textBoxTableName.KeyPress += nameTextBox_KeyPress;
        }

        private void InitializeComponent()
        {
            // FlowLayoutPanel für die Eingabefelder
            this.flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                WrapContents = false
            };

            // TextBoxen und Labels
            this.textBoxEntryName = new TextBox
            {
                Width = 220,
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(5),
            };

            this.textBoxTableName = new TextBox
            {
                Width = 220,
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(5),
            };

            this.labelEntryName = new Label
            {
                Text = "Datensatzname:",
                Font = new Font("Segoe UI", 9)
            };

            this.labelTableName = new Label
            {
                Text = "Tabellenname:",
                Font = new Font("Segoe UI", 9)
            };

            this.buttonOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(255, 255, 255),
                ForeColor = Color.Black
            };

            this.buttonCancel = new Button
            {
                Text = "Abbrechen",
                DialogResult = DialogResult.Cancel,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(255, 255, 255),
                ForeColor = Color.Black
            };

            // FlowLayoutPanel für die Buttons
            this.buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,  // Buttons nebeneinander anordnen
                AutoSize = true,
                Padding = new Padding(10),
                WrapContents = false
            };

            // Buttons zum Button-Panel hinzufügen
            this.buttonPanel.Controls.Add(this.buttonOK);
            this.buttonPanel.Controls.Add(this.buttonCancel);

            // Steuerelemente zum Eingabe-Panel hinzufügen
            this.flowLayoutPanel.Controls.Add(this.labelEntryName);
            this.flowLayoutPanel.Controls.Add(this.textBoxEntryName);
            this.flowLayoutPanel.Controls.Add(this.labelTableName);
            this.flowLayoutPanel.Controls.Add(this.textBoxTableName);
            this.flowLayoutPanel.Controls.Add(this.buttonPanel);  // Button-Panel einfügen

            // FlowLayoutPanel zum Formular hinzufügen
            this.Controls.Add(this.flowLayoutPanel);

            // Weitere Form-Attribute
            this.AcceptButton = buttonOK;
            this.CancelButton = buttonCancel;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MinimumSize = new Size(250, 220);
        }

        private TextBox textBoxEntryName;
        private TextBox textBoxTableName;
        private Label labelEntryName;
        private Label labelTableName;
        private Button buttonOK;
        private Button buttonCancel;
        private FlowLayoutPanel flowLayoutPanel;
        private FlowLayoutPanel buttonPanel; // Panel für Buttons

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
