using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Query
{
    public interface IQueryCache
    {
        Task<TResult> GetData<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
        Task SetData<TResult>(IQuery<TResult> query, TResult data, CancellationToken cancellationToken = default);
        Task Clear<TQuery, TResult>(CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
    }
}
