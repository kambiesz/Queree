using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class CronAttribute : Attribute
    {
        public string Queue { get; set; }

        public int RetryAttempts { get; set; }

        public abstract string GetCronExpression();
    }
}
