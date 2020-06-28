namespace Perigee.Framework.Cqrs.Transactions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHandleCommand<in TCommand> where TCommand : IDefineCommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}