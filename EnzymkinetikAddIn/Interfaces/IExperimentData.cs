using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Interfaces
{
    internal interface IExperimentData
    {
        string ExperimentName { get; }
        DataTable GetRawData();
    }
}
