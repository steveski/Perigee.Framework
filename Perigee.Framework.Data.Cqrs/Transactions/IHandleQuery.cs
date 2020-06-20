namespace Perigee.Cqrs.Base.Transactions
{
    using System.Threading.Tasks;

    public interface IHandleQuery<in TQuery, TResult> where TQuery : IDefineQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}