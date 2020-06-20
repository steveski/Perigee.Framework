namespace Perigee.Framework.Data.Services.Cqrs.Processors
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Data.Cqrs.Transactions;
    using Helpers.Shared;
    using SimpleInjector;

    [UsedImplicitly]
    internal sealed class QueryProcessor : IProcessQueries
    {
        private readonly Container _container;

        public QueryProcessor(Container container)
        {
            _container = container;
        }

        [DebuggerStepThrough]
        public Task<TResult> Execute<TResult>(IDefineQuery<TResult> query)
        {
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _container.GetInstance(handlerType);
            return handler.Handle((dynamic) query);
        }
    }
}