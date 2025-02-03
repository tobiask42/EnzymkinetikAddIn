using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Constants
{
    internal class ModelConstants
    {
        // Liste der Modellnamen
        public static readonly List<string> Models;

        static ModelConstants()
        {
            // Initialisierung der Liste der Modelle
            Models = new List<string>
            {
                "Michaelis-Menten",
                "Lineweaver-Burk"
            };
        }
    }
}
