namespace Perigee.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Helpers.Shared;
    using Perigee.Cqrs.Base.Transactions;
    using SimpleInjector;

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
            dynamic handler = _container.GetInstance(handlerType);
            return handler.Handle((dynamic) command);
        }
    }
}