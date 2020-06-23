namespace Perigee.Framework.Data.Cqrs
{
    using Autofac;
    using FluentValidation;
    using Validation;

    public class CqrsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            //ValidatorOptions.ResourceProviderType = typeof(Resources);

            builder.RegisterType<ValidationProcessor>().As<IProcessValidation>().SingleInstance();

            // fluent validation open generics
            // Note: not sure about the open generics. This is the the Autofac basic open generic approach
            // but the uncommented one might be the way to do the equivalent of the old SimpleInjector approach
            //builder.RegisterGeneric(typeof(NHibernateRepository<>))
            //    .As(typeof(IRepository<>))
            //    .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes()
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();

            // add unregistered type resolution for objects missing an IValidator<T>
            //container.RegisterSingleOpenGeneric(typeof(IValidator<>), typeof(ValidateNothingDecorator<>));

        }

        

    }
}