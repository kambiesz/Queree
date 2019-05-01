using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hangfire.Common;
using HF = Hangfire;

namespace Queree.BackgroundTasks.Hangfire
{
    public class HangfireTaskRunner : IBackgroundTaskRunner
    {
        private readonly IDependencyResolver _dependencyResolver;

        public HangfireTaskRunner(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void Run<TBackgroundTask>(TBackgroundTask task) where TBackgroundTask : IBackgroundTask
        {
            var handlers = _dependencyResolver.ResolveAll<IBackgroundTaskHandler<TBackgroundTask>>();
            var queueHandlers = handlers.GroupBy(x => x.GetAttribute<QueueAttribute>().Name);

            foreach (var group in queueHandlers)
            {
                string jobId = "", queue = group.Key;

                var groupedHandlers = group.GroupBy(x => x.GetAttribute<QueueAttribute>().Order);

                if (groupedHandlers.Any(x => x.Key == 0 && x.Count() > 1) && groupedHandlers.Any(x => x.Key != 0))
                    throw new Exception("Each task handler must have different order number");

                if (groupedHandlers.Any(x => x.Key != 0 && x.Count() > 1))
                    throw new Exception("Each task handler must have different order number");

                var taskHandlers = group.Select(x => new
                {
                    Handler = x,
                    Attribute = x.GetAttribute<QueueAttribute>()
                })
                .OrderBy(x => x.Attribute.Order);

                foreach (var item in taskHandlers)
                {
                    if (item.Attribute.Order == 0)
                    {
                        jobId = HF.BackgroundJob.Enqueue(() => item.Handler.Handle(task));
                    }
                    else
                    {
                        jobId = HF.BackgroundJob.ContinueJobWith(jobId, () => item.Handler.Handle(task));
                    }
                }
            }

        }

        public void Schedule<TCyclicTask>() where TCyclicTask : ICyclicTask
        {
            var task = _dependencyResolver.Resolve<TCyclicTask>();
            var attribute = task.GetAttribute<CronAttribute>();
            var cron = attribute.GetCronExpression();
            var queue = string.IsNullOrEmpty(attribute.Queue) ? "default" : attribute.Queue;

            HF.RecurringJob.AddOrUpdate(() => task.Run(), cron, queue: queue);
        }

        public void Schedule<TCyclicTask, TParams>(TParams parameters) where TCyclicTask : ICyclicTask<TParams>
        {
            var task = _dependencyResolver.Resolve<ICyclicTask<TParams>>();
            var attribute = task.GetAttribute<CronAttribute, TParams>();
            var cron = attribute.GetCronExpression();
            var queue = string.IsNullOrEmpty(attribute.Queue) ? "default" : attribute.Queue;

            HF.RecurringJob.AddOrUpdate(() => task.Run(parameters), cron, queue: queue);
        }
    }

    public class K : HF.Common.IJobFilterProvider
    {
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
