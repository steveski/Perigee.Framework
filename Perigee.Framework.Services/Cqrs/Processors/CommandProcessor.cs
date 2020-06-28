namespace Perigee.Framework.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Perigee.Framework.Base.Transactions;

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