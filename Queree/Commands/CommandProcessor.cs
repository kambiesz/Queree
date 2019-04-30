using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Queree.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IDependencyResolver _resolver;

        public CommandProcessor(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task<TResult> Process<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var handler = _resolver.Resolve<ICommandHandler<ICommand<TResult>, TResult>>();

            if (handler is null)
                throw new DependencyNotRegisteredException($"Could not resolve dependency for type: {typeof(ICommandHandler<ICommand<TResult>, TResult>)}");

            if (_resolver.CanResolve<ICommandValidator<ICommand<TResult>, TResult>>())
            {
                var validators = _resolver.ResolveAll<ICommandValidator<ICommand<TResult>, TResult>>().ToList();

                var tasks = validators.ConvertAll(x => x.Validate(command, cancellationToken));

                await Task.WhenAll(tasks);

                var results = tasks.Select(x => x.Result).ToList();

                if (results.Any(x => !x.IsValid))
                {
                    var errors = results.SelectMany(x => x.Errors).ToArray();

                    throw new InvalidCommandException($"Command of type {typeof(ICommand<TResult>)} is invalid", errors);
                }
            }

            return await handler.Handle(command);
        }
    }
}
