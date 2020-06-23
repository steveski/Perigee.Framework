namespace Perigee.Framework.Data.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Core;
    using Data.Cqrs.Transactions;
    using Helpers.Shared;

    [UsedImplicitly]
    internal sealed class CommandProcessor : IProcessCommands
    {
        private readonly Container _container;

        public CommandProcessor(Container container)
        {
            _container = container;
        }

        [DebuggerStepThrough]
        public Task Execute(IDefineCommand command)
        {
            var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());
            dynamic handler = _container.Resolve(handlerType);
            return handler.Handle((dynamic) command);
        }
    }
}