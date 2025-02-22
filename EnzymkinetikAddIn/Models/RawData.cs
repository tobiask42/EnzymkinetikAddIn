using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Data;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Models
{
    internal class RawData : IModelLogic
    {
        public Dictionary<string, DataTable> CalculateResult(string entryName)
        {
            return DataTransformer.TransformFromDatabase(entryName);
        }
    }
}
