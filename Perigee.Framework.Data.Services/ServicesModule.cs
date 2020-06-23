namespace Perigee.Framework.Data.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using Autofac.Core;
    using Cqrs.Decorators;
    using Cqrs.Processors;
    using Data.Cqrs.Transactions;
    using Module = Autofac.Module;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterQueryTransactions(builder);
            RegisterCommandTransactions(builder);

        }


        public void RegisterQueryTransactions(ContainerBuilder builder)
        {
            builder.RegisterType<QueryProcessor>().As<IProcessQueries>().SingleInstance();

            var assemblies = new[] { Assembly.GetAssembly(typeof(IHandleQuery<,>)) };
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t =>
                    t.IsClass &&
                    t.IsAbstract == false &&
                    t == typeof(IHandleQuery<,>));
            
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

        public void RegisterCommandTransactions(ContainerBuilder builder)
        {
            builder.RegisterType<CommandProcessor>().As<IProcessCommands>().SingleInstance();

            var assemblies = new[] { Assembly.GetAssembly(typeof(IHandleCommand<>)) };
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t =>
                    t.IsClass &&
                    t.IsAbstract == false &&
                    t == typeof(IHandleCommand<>));

            builder.RegisterDecorator(
                typeof(TransactionCommandHandlerDecorator<>),
                typeof(IHandleCommand<>));

            builder.RegisterDecorator(
                typeof(DeadlockRetryCommandHandlerDecorator<>),
                typeof(IHandleCommand<>));

            //builder.RegisterSingleDecorator(
            //    typeof(IHandleCommand<>),
            //    typeof(CommandLifetimeScopeDecorator<>)
            //);
            //builder.RegisterSingleDecorator(
            //    typeof(IHandleCommand<>),
            //    typeof(CommandNotNullDecorator<>)
            //);
        }


    }
}