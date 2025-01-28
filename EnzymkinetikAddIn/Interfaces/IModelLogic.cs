using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Interfaces
{
    internal interface IModelLogic
    {
        void ConfigureColumns(DataGridView dataGridView);
        void PerformCalculations(DataGridView dataGridView);
    }
}
