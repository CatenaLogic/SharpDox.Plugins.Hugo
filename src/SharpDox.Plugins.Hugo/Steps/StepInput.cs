// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepInput.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    using Model;

    internal static class StepInput
    {
        public static SDProject SDProject { get; set; }
        public static string OutputPath { get; set; }
        public static string CurrentLanguage { get; set; }
        public static HugoStrings DocStrings { get; set; }
        public static HugoStrings HugoStrings { get; set; }
        public static HugoConfig Config { get; set; }

        public static void InitStepinput(SDProject sdProject, string outputPath, string currentLanguage,
            HugoStrings docStrings, HugoStrings htmlStrings, HugoConfig config)
        {
            SDProject = sdProject;
            OutputPath = outputPath;
            CurrentLanguage = currentLanguage;
            DocStrings = docStrings;
            HugoStrings = htmlStrings;
            Config = config;
        }
    }
}