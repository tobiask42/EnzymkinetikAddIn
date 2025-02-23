using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Constants;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Models;

namespace EnzymkinetikAddIn.Factories
{
    internal class ModelFactory
    {
        private readonly Dictionary<string, Func<IModelLogic>> _modelRegistry;

        public ModelFactory()
        {
            _modelRegistry = new Dictionary<string, Func<IModelLogic>>
            {
                { ModelConstants.Models[0], () => new RawData() },
                { ModelConstants.Models[1], () => new BasicAnalysis() },
                { ModelConstants.Models[2], () => new MichaelisMenten() },
                { ModelConstants.Models[3], () => new LineweaverBurk() }
            };
        }

        public IModelLogic GenerateModel(string modelName)
        {
            if (_modelRegistry.TryGetValue(modelName, out Func<IModelLogic> modelCreator))
            {
                return modelCreator();
            }
            throw new ArgumentException($"Unbekanntes Modell: {modelName}");
        }
    }
}
