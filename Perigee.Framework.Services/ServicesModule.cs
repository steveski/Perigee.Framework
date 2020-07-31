namespace Perigee.Framework.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using Autofac;
    using Cqrs.Decorators;
    using Cqrs.Processors;
    using FluentValidation;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.Base.Validation;
    using Perigee.Framework.Services.User;
    using Perigee.Framework.Services.Validation;
    using Module = Autofac.Module;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultDateTimeService>().As<IDateTimeService>().InstancePerLifetimeScope();

            builder.Register(c =>
                {
                    var principalProvider = c.Resolve<IPrincipalProvider>();
                    return new UserService(principalProvider);
                }).As<IUserService>()
                .InstancePerLifetimeScope();


            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            RegisterValidation(builder, assemblies);
            RegisterQueryTransactions(builder, assemblies);
            RegisterCommandTransactions(builder, assemblies);


        }

        private void RegisterValidation(ContainerBuilder builder, Assembly[] assemblies)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            //ValidatorOptions.ResourceProviderType = typeof(Resources);

            builder.RegisterType<ValidationProcessor>().As<IProcessValidation>().SingleInstance();

            // fluent validation open generics
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IValidator<>));

            // add unregistered type resolution for objects missing an IValidator<T>
            builder
                .RegisterGeneric(typeof(ValidateNothingDecorator<>))
                .As(typeof(IValidator<>))
                .SingleInstance();

        }

        public void RegisterQueryTransactions(ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterType<QueryProcessor>().As<IProcessQueries>().SingleInstance();

            builder.RegisterAssemblyTypes(assemblies).AsClosedTypesOf(typeof(IHandleQuery<,>));

            builder.RegisterGenericDecorator(
                typeof(ValidateQueryDecorator<,>),
                typeof(IHandleQuery<,>)
            );

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
                typeof(ValidateCommandDecorator<>),
                typeof(IHandleCommand<>));

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