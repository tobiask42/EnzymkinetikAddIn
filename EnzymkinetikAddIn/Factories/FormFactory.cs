using System;
using System.Collections.Generic;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Interfaces;
using EnzymkinetikAddIn.Generators;
using System.Linq;
using EnzymkinetikAddIn.Constants;

namespace EnzymkinetikAddIn.Factories
{
    public class FormFactory
    {
        private readonly Dictionary<string, IFormGenerator> _generators = new Dictionary<string, IFormGenerator>();
        private readonly Dictionary<string, IModelLogic> _logics = new Dictionary<string, IModelLogic>();

        public FormFactory()
        {
            //foreach (var model in ModelConstants.Models)
            //{
            //    RegisterModel(model.Key, model.Value.GeneratorClass, model.Value.LogicClass);
            //}
        }


        //private void RegisterModel(string modelName, string generatorClassName, string logicClassName)
        //{
        //    var generatorType = Type.GetType(generatorClassName);
        //    var logicType = Type.GetType(logicClassName);

        //    if (generatorType == null || logicType == null)
        //    {
        //        throw new InvalidOperationException($"Typen für Modell '{modelName}' nicht gefunden. Generator: {generatorClassName}, Logik: {logicClassName}");
        //    }

        //    var generator = Activator.CreateInstance(generatorType) as IFormGenerator;
        //    var logic = Activator.CreateInstance(logicType) as IModelLogic;

        //    if (generator == null || logic == null)
        //    {
        //        throw new InvalidOperationException($"Fehler beim Erstellen von Instanzen für '{modelName}'.");
        //    }

        //    _generators[modelName] = generator;
        //    _logics[modelName] = logic;
        //}




        public BaseModelForm CreateForm(string modelType)
        {
            if (_generators.TryGetValue(modelType, out var generator))
            {
                return generator.GenerateForm();
            }
            throw new ArgumentException($"Unbekannter Modelltyp: {modelType}");
        }

        public IEnumerable<string> GetAvailableModels()
        {
            return _generators.Keys.Where(key => key != "Kalibriergerade");
        }
    }
}
