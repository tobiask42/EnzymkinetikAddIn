using System;

namespace EnzymkinetikAddIn.Ribbon
{
    partial class EnzymRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public EnzymRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen, andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.comboBoxEnzyme = this.Factory.CreateRibbonComboBox();
            this.comboBoxSubstrate = this.Factory.CreateRibbonComboBox();
            this.group4 = this.Factory.CreateRibbonGroup();
            this.comboBoxDataSet = this.Factory.CreateRibbonComboBox();
            this.comboBoxModelSelection = this.Factory.CreateRibbonComboBox();
            this.group5 = this.Factory.CreateRibbonGroup();
            this.comboBoxOrientation = this.Factory.CreateRibbonComboBox();
            this.checkBoxSeparate = this.Factory.CreateRibbonCheckBox();
            this.tab1.SuspendLayout();
            this.group3.SuspendLayout();
            this.group4.SuspendLayout();
            this.group5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group3);
            this.tab1.Groups.Add(this.group4);
            this.tab1.Groups.Add(this.group5);
            this.tab1.Label = "Enzymkinetik";
            this.tab1.Name = "tab1";
            // 
            // group3
            // 
            this.group3.Items.Add(this.comboBoxEnzyme);
            this.group3.Items.Add(this.comboBoxSubstrate);
            this.group3.Label = "group3";
            this.group3.Name = "group3";
            // 
            // comboBoxEnzyme
            // 
            this.comboBoxEnzyme.Label = "Enzymonzentrationen";
            this.comboBoxEnzyme.Name = "comboBoxEnzyme";
            this.comboBoxEnzyme.Text = null;
            // 
            // comboBoxSubstrate
            // 
            this.comboBoxSubstrate.Label = "Substratkonzentrationen";
            this.comboBoxSubstrate.Name = "comboBoxSubstrate";
            this.comboBoxSubstrate.Text = null;
            // 
            // group4
            // 
            this.group4.Items.Add(this.comboBoxDataSet);
            this.group4.Items.Add(this.comboBoxModelSelection);
            this.group4.Label = "group4";
            this.group4.Name = "group4";
            // 
            // comboBoxDataSet
            // 
            this.comboBoxDataSet.Label = "Datensatz";
            this.comboBoxDataSet.Name = "comboBoxDataSet";
            this.comboBoxDataSet.Text = null;
            // 
            // comboBoxModelSelection
            // 
            this.comboBoxModelSelection.Label = "Modell";
            this.comboBoxModelSelection.Name = "comboBoxModelSelection";
            this.comboBoxModelSelection.Text = null;
            // 
            // group5
            // 
            this.group5.Items.Add(this.comboBoxOrientation);
            this.group5.Items.Add(this.checkBoxSeparate);
            this.group5.Label = "group5";
            this.group5.Name = "group5";
            // 
            // comboBoxOrientation
            // 
            this.comboBoxOrientation.Label = "Ausrichtung";
            this.comboBoxOrientation.Name = "comboBoxOrientation";
            this.comboBoxOrientation.Text = null;
            // 
            // checkBoxSeparate
            // 
            this.checkBoxSeparate.Label = "Tabellen auftrennen";
            this.checkBoxSeparate.Name = "checkBoxSeparate";
            // 
            // EnzymRibbon
            // 
            this.Name = "EnzymRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.EnzymRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.group4.ResumeLayout(false);
            this.group4.PerformLayout();
            this.group5.ResumeLayout(false);
            this.group5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBoxEnzyme;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBoxSubstrate;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group4;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBoxDataSet;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBoxModelSelection;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group5;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox comboBoxOrientation;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBoxSeparate;
    }

    partial class ThisRibbonCollection
    {
        internal EnzymRibbon EnzymRibbon
        {
            get { return this.GetRibbon<EnzymRibbon>(); }
        }

        private T GetRibbon<T>()
        {
            throw new NotImplementedException();
        }
    }
}
