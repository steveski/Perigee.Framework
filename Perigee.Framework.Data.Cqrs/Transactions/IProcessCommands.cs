namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessCommands
    {
        Task Execute(IDefineCommand command);
    }
}