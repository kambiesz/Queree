using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Commands
{
    public interface ICommandProcessor
    {
        Task<TResult> Process<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    }
}
