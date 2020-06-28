namespace Perigee.Framework.Cqrs.Transactions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProcessCommandPopulation
    {
        Task<bool> Execute(IDefineCommand command, CancellationToken cancellationToken);
    }
}