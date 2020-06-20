namespace Perigee.Cqrs.Base.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessQueries
    {
        Task<TResult> Execute<TResult>(IDefineQuery<TResult> query);
    }
}