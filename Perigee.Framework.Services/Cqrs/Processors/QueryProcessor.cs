namespace Perigee.Framework.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Perigee.Framework.Base.Transactions;

    internal sealed class QueryProcessor : IProcessQueries
    {
        private readonly ILifetimeScope _lifetimeScope;

        public QueryProcessor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;

        }

        [DebuggerStepThrough]
        public Task<TResult> Execute<TResult>(IDefineQuery<TResult> query, CancellationToken cancellationToken)
        {
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _lifetimeScope.Resolve(handlerType);
            return handler.Handle((dynamic) query, cancellationToken);
        }
    }
}