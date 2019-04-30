using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Queree.Commands
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
}
