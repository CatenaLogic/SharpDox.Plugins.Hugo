// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepBase.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2017 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace SharpDox.Plugins.Hugo.Steps
{
    using System;

    internal abstract class StepBase
    {
        protected StepBase(StepRange stepRange)
        {
            StepRange = stepRange;
        }

        public StepRange StepRange { private set; get; }

        public event Action<string> OnStepMessage;
        public event Action<int> OnStepProgress;

        public abstract void RunStep();

        protected void ExecuteOnStepMessage(string message)
        {
            if (OnStepMessage != null)
            {
                OnStepMessage(message);
            }
        }

        protected void ExecuteOnStepProgress(int value)
        {
            if (OnStepProgress != null)
            {
                OnStepProgress(StepRange.GetProgressByStepProgress(value));
            }
        }
    }
}