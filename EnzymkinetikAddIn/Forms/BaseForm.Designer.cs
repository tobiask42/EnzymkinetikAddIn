namespace EnzymkinetikAddIn.Forms
{
    partial class BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridViewInputData = new System.Windows.Forms.DataGridView();
            this.labelTimeUnit = new System.Windows.Forms.Label();
            this.comboBoxTimeUnit = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.comboBoxTableName = new System.Windows.Forms.ComboBox();
            this.comboBoxConcentration = new System.Windows.Forms.ComboBox();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelConc = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.labelTables = new System.Windows.Forms.Label();
            this.deleteContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableSaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputData)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewInputData
            // 
            this.dataGridViewInputData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewInputData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInputData.Location = new System.Drawing.Point(49, 57);
            this.dataGridViewInputData.Name = "dataGridViewInputData";
            this.dataGridViewInputData.Size = new System.Drawing.Size(686, 303);
            this.dataGridViewInputData.TabIndex = 0;
            // 
            // labelTimeUnit
            // 
            this.labelTimeUnit.AutoSize = true;
            this.labelTimeUnit.Location = new System.Drawing.Point(118, 9);
            this.labelTimeUnit.Name = "labelTimeUnit";
            this.labelTimeUnit.Size = new System.Drawing.Size(56, 13);
            this.labelTimeUnit.TabIndex = 1;
            this.labelTimeUnit.Text = "Zeiteinheit";
            // 
            // comboBoxTimeUnit
            // 
            this.comboBoxTimeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTimeUnit.FormattingEnabled = true;
            this.comboBoxTimeUnit.Location = new System.Drawing.Point(121, 30);
            this.comboBoxTimeUnit.Name = "comboBoxTimeUnit";
            this.comboBoxTimeUnit.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTimeUnit.TabIndex = 2;
            this.comboBoxTimeUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeUnit_SelectedIndexChanged);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(378, 405);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(100, 20);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameTextBox_KeyPress);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(660, 402);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Datensatz Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(294, 401);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "Löschen";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // comboBoxTableName
            // 
            this.comboBoxTableName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTableName.FormattingEnabled = true;
            this.comboBoxTableName.Location = new System.Drawing.Point(502, 30);
            this.comboBoxTableName.Name = "comboBoxTableName";
            this.comboBoxTableName.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTableName.TabIndex = 6;
            this.comboBoxTableName.SelectedIndexChanged += new System.EventHandler(this.comboBoxTableName_SelectedIndexChanged);
            // 
            // comboBoxConcentration
            // 
            this.comboBoxConcentration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConcentration.FormattingEnabled = true;
            this.comboBoxConcentration.Location = new System.Drawing.Point(248, 30);
            this.comboBoxConcentration.Name = "comboBoxConcentration";
            this.comboBoxConcentration.Size = new System.Drawing.Size(121, 21);
            this.comboBoxConcentration.TabIndex = 7;
            this.comboBoxConcentration.SelectedIndexChanged += new System.EventHandler(this.comboBoxConcentration_SelectedIndexChanged);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Location = new System.Drawing.Point(375, 30);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(121, 21);
            this.comboBoxUnit.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // labelConc
            // 
            this.labelConc.AutoSize = true;
            this.labelConc.Location = new System.Drawing.Point(248, 9);
            this.labelConc.Name = "labelConc";
            this.labelConc.Size = new System.Drawing.Size(84, 13);
            this.labelConc.TabIndex = 10;
            this.labelConc.Text = "Konzentrationen";
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(375, 11);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(39, 13);
            this.labelUnit.TabIndex = 11;
            this.labelUnit.Text = "Einheit";
            // 
            // labelTables
            // 
            this.labelTables.AutoSize = true;
            this.labelTables.Location = new System.Drawing.Point(502, 9);
            this.labelTables.Name = "labelTables";
            this.labelTables.Size = new System.Drawing.Size(42, 13);
            this.labelTables.TabIndex = 12;
            this.labelTables.Text = "Tabelle";
            // 
            // deleteContextMenuStrip
            // 
            this.deleteContextMenuStrip.Name = "contextMenuStrip1";
            this.deleteContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // tableSaveButton
            // 
            this.tableSaveButton.Location = new System.Drawing.Point(505, 401);
            this.tableSaveButton.Name = "tableSaveButton";
            this.tableSaveButton.Size = new System.Drawing.Size(126, 23);
            this.tableSaveButton.TabIndex = 13;
            this.tableSaveButton.Text = "Tabelle hinzufügen";
            this.tableSaveButton.UseVisualStyleBackColor = true;
            this.tableSaveButton.Click += new System.EventHandler(this.tableSaveButton_Click);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableSaveButton);
            this.Controls.Add(this.labelTables);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.labelConc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxUnit);
            this.Controls.Add(this.comboBoxConcentration);
            this.Controls.Add(this.comboBoxTableName);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.comboBoxTimeUnit);
            this.Controls.Add(this.labelTimeUnit);
            this.Controls.Add(this.dataGridViewInputData);
            this.Name = "BaseForm";
            this.Text = "BaseModelForm";
            this.Load += new System.EventHandler(this.BaseModelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInputData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewInputData;
        private System.Windows.Forms.Label labelTimeUnit;
        private System.Windows.Forms.ComboBox comboBoxTimeUnit;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.ComboBox comboBoxTableName;
        private System.Windows.Forms.ComboBox comboBoxConcentration;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelConc;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label labelTables;
        private System.Windows.Forms.ContextMenuStrip deleteContextMenuStrip;
        private System.Windows.Forms.Button tableSaveButton;
    }
}