using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HourIntervalAttribute : CronAttribute
    {
        public int Value { get; }
        public HourIntervalAttribute(int value)
        {
            Value = value;
        }

        public override string GetCronExpression()
        {
            return $"0 0 */{Value} ? * *";
        }
    }
}
