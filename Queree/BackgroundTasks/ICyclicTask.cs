using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Queree.BackgroundTasks
{
    public interface ICyclicTask : IBackgroundTask
    {
        Task Run();
    }

    public interface ICyclicTask<TParams> : IBackgroundTask
    {
        Task Run(TParams parameters);
    }
}
