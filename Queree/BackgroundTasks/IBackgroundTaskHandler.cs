using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Queree.BackgroundTasks
{
    public interface IBackgroundTaskHandler
    {

    }

    public interface IBackgroundTaskHandler<TBackgroundTask> : IBackgroundTaskHandler where TBackgroundTask : IBackgroundTask
    {
        Task Handle(TBackgroundTask task);
    }
}
