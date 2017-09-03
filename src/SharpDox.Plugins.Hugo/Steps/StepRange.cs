// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepRange.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    internal class StepRange
    {
        public StepRange(int progressStart, int progressEnd)
        {
            ProgressStart = progressStart;
            ProgressEnd = progressEnd;
        }

        public int ProgressStart { get; private set; }
        public int ProgressEnd { get; private set; }

        public int GetProgressByStepProgress(int stepProgress)
        {
            return (int) ((((ProgressEnd - ProgressStart) / 100d) * stepProgress) + ProgressStart);
        }
    }
}