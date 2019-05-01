using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MinuteIntervalAttribute : CronAttribute
    {
        public int Value { get; }

        public MinuteIntervalAttribute(int value)
        {
            Value = value;
        }

        public override string GetCronExpression()
        {
            return $"0 */{Value} * ? * *";
        }
    }
}
