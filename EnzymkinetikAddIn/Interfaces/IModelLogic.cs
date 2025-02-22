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
        Dictionary<string, DataTable> CalculateResult(string tableName);
    }
}
