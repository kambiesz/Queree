using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Events
{
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken token = default);
    }
}
