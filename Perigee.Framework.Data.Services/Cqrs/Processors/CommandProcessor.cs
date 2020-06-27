namespace Perigee.Framework.Data.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Data.Cqrs.Transactions;
    using Helpers.Shared;

    [UsedImplicitly]
    internal sealed class CommandProcessor : IProcessCommands
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CommandProcessor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        [DebuggerStepThrough]
        public Task Execute(IDefineCommand command, CancellationToken cancellationToken)
        {
            var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());
            dynamic handler = _lifetimeScope.Resolve(handlerType);
            return handler.Handle((dynamic) command, cancellationToken);
        }
    }
}