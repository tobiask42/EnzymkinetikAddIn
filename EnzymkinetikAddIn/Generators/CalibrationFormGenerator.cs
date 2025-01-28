using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Logic;

namespace EnzymkinetikAddIn.Generators
{
    internal class CalibrationFormGenerator : IFormGenerator
    {
        public BaseModelForm GenerateForm()
        {
            // Erstellt ein neues Formular
            BaseModelForm form = new BaseModelForm();

            CalibrationLogic curveLogic = new CalibrationLogic();

            // Zugriff auf das DataGridView des Formulars
            var dataGridView = form.GetDataGridView();

            curveLogic.ConfigureColumns(dataGridView);
            // Gibt das konfigurierte Formular zurück
            return form;
        }
    }
}
