using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Queree.BackgroundTasks
{
    public static class Extensions
    {
        public static TAttribute GetAttribute<TAttribute>(this IBackgroundTaskHandler task) where TAttribute : Attribute
        {
            return task.GetType().GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetAttribute<TAttribute>(this ICyclicTask task) where TAttribute : Attribute
        {
            return task.GetType().GetCustomAttribute<TAttribute>();
        }

        public static TAttribute GetAttribute<TAttribute, TParams>(this ICyclicTask<TParams> task) where TAttribute : Attribute
        {
            return task.GetType().GetCustomAttribute<TAttribute>();
        }
    }
}
