using System.Threading.Tasks;

namespace Queree.Query
{
    public class QueryRunner : IQueryRunner
    {
        private readonly IDependencyResolver _resolver;
        private readonly IQueryCache _queryCache;

        public QueryRunner(IDependencyResolver resolver, IQueryCache queryCache)
        {
            _resolver = resolver;
            _queryCache = queryCache;
        }

        public async Task<TResult> Run<TResult>(IQuery<TResult> query)
        {
            var attribute = query.GetAttribute();
            TResult result = default;

            if (attribute != null)
            {
                result = await _queryCache.GetData(query);
            }

            if (result is null)
            {
                var handler = _resolver.Resolve<IQueryHandler<IQuery<TResult>, TResult>>();

                result = await handler.Handle(query);

                if (attribute != null && result != null)
                {
                    await _queryCache.SetData(query, result);
                }
            }

            return result;
        }
    }
}
