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
            this.dropDownConcentrations = this.Factory.CreateRibbonDropDown();
            this.dropDownUnit = this.Factory.CreateRibbonDropDown();
            this.buttonOpenInputForm = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.dropDownDataSet = this.Factory.CreateRibbonDropDown();
            this.dropDownModel = this.Factory.CreateRibbonDropDown();
            this.buttonEditData = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.buttonGenerateResult = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.group3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = "Enzymkinetik";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.dropDownConcentrations);
            this.group1.Items.Add(this.dropDownUnit);
            this.group1.Items.Add(this.buttonOpenInputForm);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // dropDownConcentrations
            // 
            this.dropDownConcentrations.Label = "Konzentrationen";
            this.dropDownConcentrations.Name = "dropDownConcentrations";
            // 
            // dropDownUnit
            // 
            this.dropDownUnit.Label = "Einheit";
            this.dropDownUnit.Name = "dropDownUnit";
            // 
            // buttonOpenInputForm
            // 
            this.buttonOpenInputForm.Label = "Datensatz erstellen";
            this.buttonOpenInputForm.Name = "buttonOpenInputForm";
            this.buttonOpenInputForm.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonOpenInputForm_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.dropDownDataSet);
            this.group2.Items.Add(this.dropDownModel);
            this.group2.Items.Add(this.buttonEditData);
            this.group2.Label = "group2";
            this.group2.Name = "group2";
            // 
            // dropDownDataSet
            // 
            this.dropDownDataSet.Label = "Datensatz";
            this.dropDownDataSet.Name = "dropDownDataSet";
            // 
            // dropDownModel
            // 
            this.dropDownModel.Label = "Modell";
            this.dropDownModel.Name = "dropDownModel";
            // 
            // buttonEditData
            // 
            this.buttonEditData.Label = "Datensatz Bearbeiten";
            this.buttonEditData.Name = "buttonEditData";
            this.buttonEditData.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonEditData_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.buttonGenerateResult);
            this.group3.Label = "group3";
            this.group3.Name = "group3";
            // 
            // buttonGenerateResult
            // 
            this.buttonGenerateResult.Label = "Tabellen Generieren";
            this.buttonGenerateResult.Name = "buttonGenerateResult";
            this.buttonGenerateResult.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonGenerateResult_Click);
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
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonOpenInputForm;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonEditData;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonGenerateResult;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownConcentrations;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownDataSet;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownModel;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown dropDownUnit;
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