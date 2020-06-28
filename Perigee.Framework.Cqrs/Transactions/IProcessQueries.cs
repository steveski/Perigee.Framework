namespace Perigee.Framework.Cqrs.Transactions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProcessQueries
    {
        Task<TResult> Execute<TResult>(IDefineQuery<TResult> query, CancellationToken cancellationToken);
    }
}