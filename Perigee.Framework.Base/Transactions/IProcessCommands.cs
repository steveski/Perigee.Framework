namespace Perigee.Framework.Base.Transactions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProcessCommands
    {
        Task Execute(IDefineCommand command, CancellationToken cancellationToken);
    }
}