using Queree.Events;
using Queree.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.Events
{
    public class QueryReloadEvent<TQuery, TResult> : IEvent where TQuery : IQuery<TResult>
    {
    }
}
