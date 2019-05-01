using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks.Hangfire
{
    public class HfDeleteOnSuccededAttribute : JobFilterAttribute, IElectStateFilter
    {
        public void OnStateElection(ElectStateContext context)
        {
            if (!(context.CandidateState is SucceededState state))
            {
                return;
            }

            context.CandidateState = new DeletedState()
            {
                Reason = "Job has deleted by attribute after succeeded run"
            };
        }
    }
}
