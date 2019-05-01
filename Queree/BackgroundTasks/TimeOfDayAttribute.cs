using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.BackgroundTasks
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TimeOfDayAttribute : CronAttribute
    {
        public int Hour { get; }
        public int Minute { get; }

        public TimeOfDayAttribute(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public override string GetCronExpression()
        {
            return $"0 {Minute} {Hour} ? * *";
        }
    }
}
