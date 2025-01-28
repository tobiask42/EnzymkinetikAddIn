using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Logic;

namespace EnzymkinetikAddIn.Generators
{
    internal class MichaelisMentenGenerator : IFormGenerator
    {
        public BaseModelForm GenerateForm()
        {
            // Erstellt ein neues Formular
            BaseModelForm form = new BaseModelForm();

            MichaelisMentenLogic mentenLogic = new MichaelisMentenLogic();

            // Zugriff auf das DataGridView des Formulars
            var dataGridView = form.GetDataGridView();

            mentenLogic.ConfigureColumns(dataGridView);

            return form;
        }
    }
}
