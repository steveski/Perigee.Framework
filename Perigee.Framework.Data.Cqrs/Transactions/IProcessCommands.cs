namespace Perigee.Cqrs.Base.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessCommands
    {
        Task Execute(IDefineCommand command);
    }
}