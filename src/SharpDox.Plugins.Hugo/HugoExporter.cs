// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HugoExporter.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Model;
    using Sdk.Exporter;
    using Sdk.Local;
    using Steps;

    public class HugoExporter : IExporter
    {
        private readonly HugoConfig _config;

        private readonly ILocalController _localController;
        private readonly HugoStrings _strings;

        private double _docCount;
        private double _docIndex;

        public HugoExporter(ILocalController localController, HugoConfig config)
        {
            _localController = localController;
            _strings = localController.GetLocalStrings<HugoStrings>();
            _config = config;
        }

        public event Action<string> OnRequirementsWarning;
        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;

        public bool CheckRequirements()
        {
            return true;

            //if (_htmlConfig.Theme == null)
            //{
            //    ExecuteOnRequirementsWarning(_docNetStrings.ThemeMissing);
            //}
            //return _htmlConfig.Theme != null;
        }

        public void Export(SDProject sdProject, string outputPath)
        {
            _docCount = sdProject.DocumentationLanguages.Count;
            _docIndex = 0;

            foreach (var docLanguage in sdProject.DocumentationLanguages)
            {
                StepInput.InitStepinput(sdProject, Path.Combine(outputPath), docLanguage, 
                    _localController.GetLocalStringsOrDefault<HugoStrings>(docLanguage), _strings, _config);

                var steps = new List<StepBase>();
                steps.Add(new PreStep(0, 5));
                //steps.Add(new CopyResourcesStep(25, 25));
                steps.Add(new CreateDataStep(50, 100)
                {
	                IgnorePrivateMembers = _config.IgnorePrivateMembers
                });

                foreach (var step in steps)
                {
                    ExecuteOnStepProgress(step.StepRange.ProgressStart);

                    step.OnStepMessage += ExecuteOnStepMessage;
                    step.OnStepProgress += ExecuteOnStepProgress;
                    step.RunStep();

                    ExecuteOnStepProgress(step.StepRange.ProgressEnd);
                }

                _docIndex++;
            }
        }

        public string ExporterName
        {
            get { return "Hugo"; }
        }

        internal void ExecuteOnStepMessage(string message)
        {
            var handler = OnStepMessage;
            if (handler != null)
            {
                handler($"({StepInput.CurrentLanguage}) - {message}");
            }
        }

        internal void ExecuteOnStepProgress(int progress)
        {
            var handler = OnStepProgress;
            if (handler != null)
            {
                handler((int) ((progress / _docCount) + (100 / _docCount * _docIndex)));
            }
        }

        internal void ExecuteOnRequirementsWarning(string message)
        {
            var handler = OnRequirementsWarning;
            if (handler != null)
            {
                handler(message);
            }
        }
    }
}