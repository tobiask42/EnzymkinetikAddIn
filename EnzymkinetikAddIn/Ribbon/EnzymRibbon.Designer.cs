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
            this.group1 = this.Factory.CreateRibbonGroup();
            this.dropDownModelSelection = this.Factory.CreateRibbonDropDown();
            this.buttonModelChoice = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.buttonAddCalibration = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Label = "Enzymkinetik";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.dropDownModelSelection);
            this.group1.Items.Add(this.buttonModelChoice);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // dropDownModelSelection
            // 
            this.dropDownModelSelection.Label = "Modellauswahl";
            this.dropDownModelSelection.Name = "dropDownModelSelection";
            this.dropDownModelSelection.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.dropDownModelSelection_SelectionChanged);
            // 
            // buttonModelChoice
            // 
            this.buttonModelChoice.Label = "Tabelle erstellen";
            this.buttonModelChoice.Name = "buttonModelChoice";
            this.buttonModelChoice.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonModelChoice_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.buttonAddCalibration);
            this.group2.Label = "group2";
            this.group2.Name = "group2";
            // 
            // buttonAddCalibration
            // 
            this.buttonAddCalibration.Label = "Kalibriergerade hinzufügen";
            this.buttonAddCalibration.Name = "buttonAddCalibration";
            this.buttonAddCalibration.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonAddCalibration_Click);
            // 
            // EnzymRibbon
            // 
            this.Name = "EnzymRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.EnzymRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownModelSelection;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonAddCalibration;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonModelChoice;
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
