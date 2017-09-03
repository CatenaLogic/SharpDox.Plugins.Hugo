// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreStep.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    using System.IO;

    internal class PreStep : StepBase
    {
        public PreStep(int progressStart, int progressEnd) : base(new StepRange(progressStart, progressEnd))
        {
        }

        public override void RunStep()
        {
            CreateFolders();
        }

        private void CreateFolders()
        {
            EnsureFolder(Path.Combine(StepInput.OutputPath, "images"));
            //EnsureFolder(Path.Combine(StepInput.OutputPath, "data", "images"));
            //EnsureFolder(Path.Combine(StepInput.OutputPath, "data", "articles"));
            //EnsureFolder(Path.Combine(StepInput.OutputPath, "data", "types"));
            //EnsureFolder(Path.Combine(StepInput.OutputPath, "data", "namespaces"));
        }

        private void EnsureFolder(string pathToFolder)
        {
            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }
        }
    }
}