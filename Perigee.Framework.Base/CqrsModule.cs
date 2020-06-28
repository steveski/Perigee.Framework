namespace Perigee.Framework.Base
{
    using System.Security.Claims;
    using Autofac;
    using FluentValidation;
    using Validation;

    public class CqrsModule : Module
    {
        private readonly ClaimsPrincipal _principal;

        public CqrsModule(ClaimsPrincipal principal)
        {
            _principal = principal;

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _principal)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(c => new UserService
            {
                Principal = _principal
            }).As<IUserService>()
                .InstancePerLifetimeScope();
            

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