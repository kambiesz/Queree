using Queree.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IQueryCache _queryCache;

        public EventDispatcher(IDependencyResolver dependencyResolver, IQueryCache queryCache)
        {
            _dependencyResolver = dependencyResolver;
            _queryCache = queryCache;
        }

        public Task Disptach<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent
        {
            var handlers = _dependencyResolver.ResolveAll<IEventHandler<TEvent>>().ToList();
            var tasks = handlers.ConvertAll(x => x.Handle(@event, token));

            return Task.WhenAll(tasks);
        }

        public Task Disptach<TQuery, TResult>(QueryReloadEvent<TQuery,TResult> @event, CancellationToken token = default) where TQuery : IQuery<TResult>
        {
            var handlers = _dependencyResolver.ResolveAll<IEventHandler<QueryReloadEvent<TQuery,TResult>>>().ToList();
            var tasks = handlers.ConvertAll(x => x.Handle(@event, token));

            var clearCacheTask = _queryCache.Clear<TQuery, TResult>(token);
            tasks.Add(clearCacheTask);

            return Task.WhenAll(tasks);
        }
    }
}
