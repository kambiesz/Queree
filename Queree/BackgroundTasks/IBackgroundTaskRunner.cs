using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    public interface IBackgroundTaskRunner
    {
        void Run<TBackgroundTask>(TBackgroundTask task) where TBackgroundTask : IBackgroundTask;
        void Schedule<TCyclicTask>() where TCyclicTask : ICyclicTask;
        void Schedule<TCyclicTask, TParams>(TParams parameters) where TCyclicTask : ICyclicTask<TParams>;
    }
}
