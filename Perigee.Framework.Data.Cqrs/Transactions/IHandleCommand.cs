namespace Perigee.Cqrs.Base.Transactions
{
    using System.Threading.Tasks;

    public interface IHandleCommand<in TCommand> where TCommand : IDefineCommand
    {
        Task Handle(TCommand command);
    }
}