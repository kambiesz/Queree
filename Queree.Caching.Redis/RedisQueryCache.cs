using ProtoBuf;
using Queree.Query;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Caching.Redis
{
    public class RedisQueryCache : IQueryCache
    {
        private readonly ConfigurationOptions _options;

        public RedisQueryCache(ConfigurationOptions options)
        {
            _options = options;
        }

        public async Task Clear<TQuery, TResult>(CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
        {
            using (var redis = await ConnectionMultiplexer.ConnectAsync(_options))
            {
                var db = redis.GetDatabase();
                var typeName = typeof(TQuery).Name;
                var queryKeys = await db.HashKeysAsync(typeName);

                if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

                var transaction = db.CreateTransaction();
             
                var listDeleteTask = transaction.HashDeleteAsync(typeName, queryKeys);
                var queryDeleteTask = transaction.KeyDeleteAsync(queryKeys.Select(x => new RedisKey().Append(x.ToString()))
                    .ToArray());

                await transaction.ExecuteAsync();
            }
        }

        public async Task<TResult> GetData<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            using (var redis = await ConnectionMultiplexer.ConnectAsync(_options))
            {
                var db = redis.GetDatabase();
                var key = query.GetCacheKey();
                var attribute = query.GetAttribute();

                var transaction = db.CreateTransaction();

                transaction.AddCondition(Condition.KeyExists(key));
                var getDataTask =  transaction.StringGetAsync(key);

                if (attribute.CachingType == CachingType.Sliding)
                {
                    var keyExpireTask = transaction.KeyExpireAsync(key, TimeSpan.FromSeconds(attribute.Duration));
                }

                if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

                var transactionResult = await transaction.ExecuteAsync();

                if (!transactionResult) return default;

                var data = await getDataTask;
                var buffer = Convert.FromBase64String(data);

                using (var ms = new MemoryStream(buffer))
                {
                    return Serializer.Deserialize<TResult>(ms);
                }
            }
        }

        public async Task SetData<TResult>(IQuery<TResult> query, TResult data, CancellationToken cancellationToken = default)
        {
            using (var redis = await ConnectionMultiplexer.ConnectAsync(_options))
            {
                var db = redis.GetDatabase();
                var key = query.GetCacheKey();
                var attribute = query.GetAttribute();
                var typeName = query.GetType().Name;
                string content;

                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, data);
                    content = Convert.ToBase64String(ms.ToArray());
                }

                var transaction = db.CreateTransaction();

                var setDataTast = transaction.StringSetAsync(key, content, TimeSpan.FromSeconds(attribute.Duration));
                var setCacheFieldTask = transaction.HashSetAsync(typeName, key, 1);
                var setCacheExpireTask = transaction.KeyExpireAsync(typeName, TimeSpan.FromHours(24));

                if (cancellationToken.IsCancellationRequested) cancellationToken.ThrowIfCancellationRequested();

                await transaction.ExecuteAsync();
            }
        }
    }
}
