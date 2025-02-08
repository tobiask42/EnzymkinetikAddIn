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
        public List<List<DataTable>> CalculateResult(string tableName)
        {
            DataTable rawData = DataTransformer.TransformFromDatabase(tableName);
            //rawData = DatabaseHelper.LoadTable(tableName);
            return new List<List<DataTable>>
            {
                new List<DataTable> { rawData }
            };
        }
    }
}
