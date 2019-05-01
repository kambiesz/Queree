using Hangfire.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HF = Hangfire;

namespace Queree.BackgroundTasks.Hangfire
{
    public class CustomJobFilterProvider : IJobFilterProvider
    {
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            var filters = new List<JobFilter>();

            if (job.Type.IsAssignableFrom(typeof(IBackgroundTaskHandler)))
            {
                var queueAttr = job.Type.GetCustomAttribute<QueueAttribute>();
                var deleteAttr = job.Type.GetCustomAttribute<DeleteOnSuccededAttribute>();

                filters.Add(new JobFilter(new HF.QueueAttribute(queueAttr.Name), JobFilterScope.Method, null));

                if (queueAttr.RetryAttempts > 0)
                {
                    filters.Add(new JobFilter(new HF.AutomaticRetryAttribute { Attempts = queueAttr.RetryAttempts }, JobFilterScope.Method, null));
                }

                if (deleteAttr != null)
                {
                    filters.Add(new JobFilter(new HfDeleteOnSuccededAttribute(), JobFilterScope.Method, null));
                }
            }
            else
            {
                var cronAttr = job.Type.GetCustomAttribute<CronAttribute>();

                filters.Add(new JobFilter(new HF.QueueAttribute(cronAttr.Queue), JobFilterScope.Method, null));

                if (cronAttr.RetryAttempts > 0)
                {
                    filters.Add(new JobFilter(new HF.AutomaticRetryAttribute { Attempts = cronAttr.RetryAttempts }, JobFilterScope.Method, null));
                }
            }

            return filters;
        }
    }
}
