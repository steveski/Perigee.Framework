namespace Perigee.Cqrs.Base.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessCommandPopulation
    {
        Task<bool> Execute(IDefineCommand command);
    }
}