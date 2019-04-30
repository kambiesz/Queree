using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Queree.Query
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(IQuery<TResult> query);
    }
}
