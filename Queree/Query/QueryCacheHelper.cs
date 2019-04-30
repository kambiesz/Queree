using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Queree.Query
{
    public static class QueryCacheHelper
    {
        public static string GetCacheKey<TResult>(this IQuery<TResult> query)
        {
            var queryType = query.GetType();
            var cacheAttribute = queryType.GetCustomAttribute<CacheAttribute>();

            if (cacheAttribute is null) throw new InvalidOperationException("The query doesn't contain CacheAttribute");

            var cacheKey = new StringBuilder();

            var props = queryType.GetProperties()
                .Where(x => x.GetCustomAttribute<QueryCacheKeyAttribute>() != null)
                .Select(x => new
                {
                    Property = x,
                    Attribute = x.GetCustomAttribute<QueryCacheKeyAttribute>()
                })
                .OrderBy(x => x.Property.Name);

            foreach (var prop in props)
            {
                var propValue = prop.Property.GetValue(query);
                string value;

                if (propValue is null)
                {
                    value = "";
                }
                else
                {
                    value = propValue.ToString();
                }

                cacheKey.Append("." + value);
            }

            var bytes = Encoding.UTF8.GetBytes(cacheKey.ToString());

            using (var md5 = MD5.Create())
            {
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }

        public static CacheAttribute GetAttribute<TResult>(this IQuery<TResult> query)
        {
            var queryType = query.GetType();
            var cacheAttribute = queryType.GetCustomAttribute<CacheAttribute>();

            return cacheAttribute;
        }
    }
}
