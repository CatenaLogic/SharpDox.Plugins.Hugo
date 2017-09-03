// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyResourcesStep.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class CopyResourcesStep : StepBase
    {
        public CopyResourcesStep(int progressStart, int progressEnd) 
            : base(new StepRange(progressStart, progressEnd))
        {
        }

        public override void RunStep()
        {
            var assemblyPath = Path.GetDirectoryName(GetType().Assembly.Location);

            CopyFolder(Path.Combine(assemblyPath, "resources", "images"), Path.Combine(StepInput.OutputPath, "images"));
            CopyImages(StepInput.SDProject.Images, Path.Combine(StepInput.OutputPath, "images"));
        }

        private void CopyFolder(string input, string output, bool recursive = true)
        {
            EnsureFolder(output);

            var files = Directory.EnumerateFiles(input);
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(output, Path.GetFileName(file)), true);
            }

            if (recursive)
            {
                foreach (var dir in Directory.EnumerateDirectories(input))
                {
                    CopyFolder(dir, Path.Combine(output, Path.GetFileName(dir)));
                }
            }
        }

        private void CopyImages(IEnumerable<string> images, string outputPath)
        {
            foreach (var image in images)
            {
                CopyImage(image, Path.Combine(outputPath, "images"));
            }
        }

        private void CopyImage(string imagePath, string outputPath)
        {
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                EnsureFolder(outputPath);
                File.Copy(imagePath, Path.Combine(outputPath, Path.GetFileName(imagePath)), true);
            }
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