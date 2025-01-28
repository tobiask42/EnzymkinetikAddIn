using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Constants
{
    internal class ModelConstants
    {
        public static readonly Dictionary<string, (string GeneratorClass, string LogicClass)> Models;

        static ModelConstants()
        {
            Models = new Dictionary<string, (string, string)>
        {
            { "Kalibriergerade", ("EnzymkinetikAddIn.Generators.CalibrationFormGenerator", "EnzymkinetikAddIn.Logic.CalibrationLogic") },
            { "Michaelis-Menten", ("EnzymkinetikAddIn.Generators.MichaelisMentenGenerator", "EnzymkinetikAddIn.Logic.MichaelisMentenLogic") },
            { "Lineweaver-Burk", ("EnzymkinetikAddIn.Generators.LineweaverBurkGenerator", "EnzymkinetikAddIn.Logic.LineweaverBurkLogic") }
        };
        }
    }
}