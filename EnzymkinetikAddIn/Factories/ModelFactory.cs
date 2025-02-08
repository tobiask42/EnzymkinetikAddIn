using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Models;

namespace EnzymkinetikAddIn.Factories
{
    internal class ModelFactory
    {

        public IModelLogic GenerateModel(string modelName)
        {
            switch (modelName)
            {
                case "Rohdaten":
                    return new RawData();
                case "Michaelis-Menten":
                    return new MichaelisMenten();
                case "Lineweaver-Burk":
                    return new LineweaverBurk();
                default:
                    throw new ArgumentException($"Unbekanntes Modell: {modelName}");
            }
        }
    }
}
