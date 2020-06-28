namespace Perigee.Framework.Services
{
    using System;
    using System.Reflection;
    using System.Security.Claims;
    using Autofac;
    using Cqrs.Decorators;
    using Cqrs.Processors;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.Services.User;
    using Module = Autofac.Module;

    public class ServicesModule : Module
    {
        private readonly ClaimsPrincipal _principal;

        public ServicesModule(ClaimsPrincipal principal)
        {
            _principal = principal;

        }


        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultDateTimeService>().As<IDateTimeService>().InstancePerLifetimeScope();

            builder.Register(c => _principal)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(c => new UserService
                {
                    ClaimsPrincipal = _principal
                }).As<IUserService>()
                .InstancePerLifetimeScope();


            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            RegisterQueryTransactions(builder, assemblies);
            RegisterCommandTransactions(builder, assemblies);

        }


        public void RegisterQueryTransactions(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterType<QueryProcessor>().As<IProcessQueries>().SingleInstance();

            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IHandleQuery<,>));

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

        public void RegisterCommandTransactions(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterType<CommandProcessor>().As<IProcessCommands>().SingleInstance();

            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IHandleCommand<>));

            builder.RegisterGenericDecorator(
                typeof(TransactionCommandHandlerDecorator<>),
                typeof(IHandleCommand<>));

            //////////builder.RegisterDecorator(
            //////////    typeof(DeadlockRetryCommandHandlerDecorator<>),
            //////////    typeof(IHandleCommand<>));

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