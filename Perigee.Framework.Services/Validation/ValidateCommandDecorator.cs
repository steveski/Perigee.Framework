namespace Perigee.Framework.Services.Validation
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using Perigee.Framework.Base.Transactions;

    public class ValidateCommandDecorator<TCommand> : IHandleCommand<TCommand>
        where TCommand : IDefineCommand
    {
        private readonly IHandleCommand<TCommand> _decorated;
        private readonly IValidator<TCommand> _validator;

        public ValidateCommandDecorator(IHandleCommand<TCommand> decorated, IValidator<TCommand> validator)
        {
            _decorated = decorated;
            _validator = validator;
        }

        public Task Handle(TCommand command, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(command);

            return _decorated.Handle(command, cancellationToken);
        }

    }


}
