using Queree.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Events
{
    public interface IEventDispatcher
    {
        Task Disptach<TQuery, TResult>(QueryReloadEvent<TQuery, TResult> @event, CancellationToken token = default) where TQuery : IQuery<TResult>;
        Task Disptach<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent; 
    }
}
