using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Commands
{
    public interface ICommandValidator<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<ValidationResult> Validate(TCommand command, CancellationToken cancellationToken = default);
    }
}
