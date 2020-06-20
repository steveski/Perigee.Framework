namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System.Threading.Tasks;

    public interface IProcessQueries
    {
        Task<TResult> Execute<TResult>(IDefineQuery<TResult> query);
    }
}