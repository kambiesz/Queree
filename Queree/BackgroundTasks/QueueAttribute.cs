using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public int RetryAttempts { get; set; }

        public QueueAttribute()
        {
            Name = "default";
            Order = 0;
        }
    }
}
