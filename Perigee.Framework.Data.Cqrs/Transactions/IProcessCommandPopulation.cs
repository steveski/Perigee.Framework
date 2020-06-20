namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessCommandPopulation
    {
        Task<bool> Execute(IDefineCommand command);
    }
}