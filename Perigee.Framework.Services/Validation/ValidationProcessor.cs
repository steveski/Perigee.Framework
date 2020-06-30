namespace Perigee.Framework.Services.Validation
{
    using System.Diagnostics;
    using Autofac;
    using FluentValidation;
    using FluentValidation.Results;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.Base.Validation;

    internal sealed class ValidationProcessor : IProcessValidation
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ValidationProcessor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        [DebuggerStepThrough]
        public ValidationResult Validate<TResult>(IDefineQuery<TResult> query)
        {
            var validatedType = typeof(IValidator<>).MakeGenericType(query.GetType());
            dynamic validator = _lifetimeScope.Resolve(validatedType);
            return validator.Validate((dynamic) query);
        }

        [DebuggerStepThrough]
        public ValidationResult Validate(IDefineCommand command)
        {
            var validatedType = typeof(IValidator<>).MakeGenericType(command.GetType());
            dynamic validator = _lifetimeScope.Resolve(validatedType);
            return validator.Validate((dynamic) command);
        }
    }
}