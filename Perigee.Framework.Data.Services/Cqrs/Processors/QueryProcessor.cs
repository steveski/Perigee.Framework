namespace Perigee.Framework.Data.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Autofac;
    using Data.Cqrs.Transactions;
    using Helpers.Shared;

    [UsedImplicitly]
    internal sealed class QueryProcessor : IProcessQueries
    {
        private readonly ILifetimeScope _lifetimeScope;

        public QueryProcessor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;

        }

        [DebuggerStepThrough]
        public Task<TResult> Execute<TResult>(IDefineQuery<TResult> query)
        {
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _lifetimeScope.Resolve(handlerType);
            return handler.Handle((dynamic) query);
        }
    }
}