namespace Perigee.Framework.Data.Services
{
    using System;
    using System.Reflection;
    using Cqrs.Decorators;
    using Cqrs.Processors;
    using Data.Cqrs.Transactions;
    using SimpleInjector;

    public static class CompositionRoot
    {
        // TODO: Introduce RootCompositionSettings in place of the params args
        public static void RegisterQueryTransactions(this Container container) //, params Assembly[] assemblies)
        {
            var assemblies = new[] {Assembly.GetAssembly(typeof(IHandleQuery<,>))};

            container.RegisterSingleton<IProcessQueries, QueryProcessor>();

            container.Register(typeof(IHandleQuery<,>), AppDomain.CurrentDomain.GetAssemblies());
            //container.Collection.Register(typeof(IHandleQuery<,>), assemblies);
            //container.RegisterDecorator(
            //    typeof(IHandleQuery<,>),
            //    typeof(ValidateQueryDecorator<,>)
            //);
            //container.RegisterDecorator(
            //    typeof(IHandleQuery<,>),
            //    typeof(QueryLifetimeScopeDecorator<,>),
            //    Lifestyle.Singleton
            //);
            //container.RegisterDecorator(
            //    typeof(IHandleQuery<,>),
            //    typeof(QueryNotNullDecorator<,>),
            //    Lifestyle.Singleton
            //);
        }

        public static void RegisterCommandTransactions(this Container container) //, params Assembly[] assemblies)
        {
            var assemblies = new[] {Assembly.GetAssembly(typeof(IHandleCommand<>))};

            container.RegisterSingleton<IProcessCommands, CommandProcessor>();

            container.Register(typeof(IHandleCommand<>), AppDomain.CurrentDomain.GetAssemblies());
            //container.Collection.Register(typeof(IHandleCommand<>), assemblies);
            container.RegisterDecorator(
                typeof(IHandleCommand<>),
                typeof(TransactionCommandHandlerDecorator<>));

            container.RegisterDecorator(
                typeof(IHandleCommand<>),
                typeof(DeadlockRetryCommandHandlerDecorator<>));

            //container.RegisterSingleDecorator(
            //    typeof(IHandleCommand<>),
            //    typeof(CommandLifetimeScopeDecorator<>)
            //);
            //container.RegisterSingleDecorator(
            //    typeof(IHandleCommand<>),
            //    typeof(CommandNotNullDecorator<>)
            //);
        }
    }
}