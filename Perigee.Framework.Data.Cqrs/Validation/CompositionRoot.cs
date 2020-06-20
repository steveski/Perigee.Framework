namespace Perigee.Cqrs.Base.Validation
{
    using System.Reflection;
    using FluentValidation;
    using SimpleInjector;
    using Transactions;

    public static class CompositionRoot
    {
        public static void RegisterFluentValidation(this Container container, params Assembly[] assemblies)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            //ValidatorOptions.ResourceProviderType = typeof(Resources);

            assemblies = assemblies ?? new[] {Assembly.GetAssembly(typeof(IHandleCommand<>))};

            container.RegisterSingleton<IProcessValidation, ValidationProcessor>();

            // fluent validation open generics
            container.Collection.Register(typeof(IValidator<>), assemblies);

            // add unregistered type resolution for objects missing an IValidator<T>
            //container.RegisterSingleOpenGeneric(typeof(IValidator<>), typeof(ValidateNothingDecorator<>));
        }
    }
}