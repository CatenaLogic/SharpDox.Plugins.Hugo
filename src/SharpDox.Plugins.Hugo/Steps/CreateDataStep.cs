// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDataStep.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Model.Documentation;
    using Model.Documentation.Article;
    using Model.Repository;
    using Templates.Repository;

    internal class CreateDataStep : StepBase
    {
        public CreateDataStep(int progressStart, int progressEnd)
            : base(new StepRange(progressStart, progressEnd))
        {
        }

        public bool IgnorePrivateMembers { get; set; }

        public override void RunStep()
        {
            CreateNamespaceData();
            CreateTypeData();
        }

        private void CreateNamespaceData()
        {
            ExecuteOnStepProgress(30);

            foreach (var sdSolution in StepInput.SDProject.Solutions)
            {
                var finalNamespaces = new HashSet<SDNamespace>();

                foreach (var targetNamespace in sdSolution.Value.GetAllSolutionNamespaces())
                {
                    var usedNamespace = new SDNamespace(targetNamespace.Key);
                    var hasUpdatedDetails = false;

                    foreach (var targetFxNamespace in targetNamespace.Value.Values)
                    {
                        if (!hasUpdatedDetails)
                        {
                            usedNamespace.Assemblyname = targetFxNamespace.Assemblyname;

                            hasUpdatedDetails = true;
                        }

                        foreach (var namespaceType in targetFxNamespace.Types)
                        {
                            if (!usedNamespace.Types.Any(x => x.Name.Equals(namespaceType.Name)))
                            {
                                usedNamespace.Types.Add(namespaceType);
                            }
                        }
                    }

                    finalNamespaces.Add(usedNamespace);
                }

                foreach (var targetFxNamespace in finalNamespaces)
                {
                    ExecuteOnStepMessage(string.Format(StepInput.HugoStrings.CreatingNamespaceData, targetFxNamespace.Fullname));

                    var namespaceData = new NamespaceData
                    {
                        Namespace = targetFxNamespace,
                        RootPath = StepInput.OutputPath,
                        RootPrefix = StepInput.Config.RootPrefix,
                        Weight = 10
                    };

                    var fileName = targetFxNamespace.ResolvePath(null, StepInput.OutputPath);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    var content = namespaceData.TransformText();
                    content = content.CleanUp();

                    var directoryName = Path.GetDirectoryName(fileName);
                    Directory.CreateDirectory(directoryName);

                    File.WriteAllText(fileName, content);
                }
            }
        }

        private void CreateTypeData()
        {
            ExecuteOnStepProgress(60);

            // Note: this isn't 100 % accurate. Best would be to create sets of data *per* target fx, then 
            // dynamically use this. But since we are converting this to MarkDown, we need to make a decision:
            //
            // 1. Create single file assuming most types cover all the methods
            // 2. Create separate files ([assembly]/[namespace]/[type]_[targetfx].md)
            //
            // Since we will put availability to all types (and maybe methods in the future), option 1 has been chosen 

            var allTypes = new Dictionary<string, List<Tuple<SDType, SDTargetFx>>>();

            foreach (var sdSolution in StepInput.SDProject.Solutions)
            {
                foreach (var targetFxType in sdSolution.Value.GetAllSolutionTypes())
                {
                    foreach (var keyValuePair in targetFxType.Value)
                    {
                        var typeKey = keyValuePair.Value.Fullname;
                        if (!allTypes.ContainsKey(typeKey))
                        {
                            allTypes[typeKey] = new List<Tuple<SDType, SDTargetFx>>();
                        }

                       var targetFxs = allTypes[typeKey];
                        targetFxs.Add(new Tuple<SDType, SDTargetFx>(keyValuePair.Value, keyValuePair.Key.TargetFx));
                    }
                }
            }

            foreach (var sdType in allTypes)
            {
                if (sdType.Value.Count == 0)
                {
                    continue;
                }

                var type = sdType.Value.First().Item1;
                var targetFxs = sdType.Value.Select(x => x.Item2).ToList();

                if (IgnorePrivateMembers && type.Accessibility.IsNonPublic(false))
                {
                    continue;
                }

                ExecuteOnStepMessage(string.Format(StepInput.HugoStrings.CreatingTypeData, type));

                var typeData = new TypeData
                {
                    IgnorePrivateMembers = IgnorePrivateMembers,
                    Type = type,
                    TargetFxs = targetFxs.ToArray(),
                    Weight = 20,
                    RootPath = StepInput.OutputPath,
                    RootPrefix = StepInput.Config.RootPrefix,
                };

                var fileName = type.ResolvePath(StepInput.OutputPath);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    continue;
                }

                var content = typeData.TransformText();
                content = content.CleanUp();

                var directoryName = Path.GetDirectoryName(fileName);
                Directory.CreateDirectory(directoryName);

                File.WriteAllText(fileName, content);
            }
        }
    }
}