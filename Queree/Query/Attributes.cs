using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.Query
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CacheAttribute : Attribute
    {
        private const int DEFAULT_CACHE_DURATION = 60;

        public int Duration { get; set; }
        public CachingType CachingType { get; set; }

        public CacheAttribute()
        {
            Duration = DEFAULT_CACHE_DURATION;
        }

    }

    public enum CachingType
    {
        Absolute,
        Sliding
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class QueryCacheKeyAttribute : Attribute
    {
    }
}
