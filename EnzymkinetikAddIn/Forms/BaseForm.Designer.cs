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
            this.dataGridViewInputData = new System.Windows.Forms.DataGridView();
            this.labelTimeUnit = new System.Windows.Forms.Label();
            this.comboBoxTimeUnit = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
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
            this.dataGridViewInputData.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewInputData_RowsAdded);
            this.dataGridViewInputData.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridViewInputData_RowsRemoved);
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
            this.nameTextBox.Location = new System.Drawing.Point(496, 402);
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
            this.saveButton.Text = "Speichern";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}