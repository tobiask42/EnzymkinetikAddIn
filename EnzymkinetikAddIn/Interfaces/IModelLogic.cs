using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnzymkinetikAddIn.Interfaces
{
    internal interface IModelLogic
    {
        List<List<DataTable>> CalculateResult(string tableName);
    }
}
