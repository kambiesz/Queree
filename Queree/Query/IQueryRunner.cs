using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Queree.Query
{
    public interface IQueryRunner
    {
        Task<TResult> Run<TResult>(IQuery<TResult> query);
    }
}
