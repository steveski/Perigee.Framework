namespace Perigee.Framework.Services.Validation
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using Perigee.Framework.Base.Transactions;

    public class ValidateQueryDecorator<TQuery, TResult> : IHandleQuery<TQuery, TResult>
        where TQuery : IDefineQuery<TResult>
    {
        private readonly IHandleQuery<TQuery, TResult> _decorated;
        private readonly IValidator<TQuery> _validator;

        public ValidateQueryDecorator(IHandleQuery<TQuery, TResult> decorated, IValidator<TQuery> validator)
        {
            _decorated = decorated;
            _validator = validator;
        }

        public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(query);

            return _decorated.Handle(query, cancellationToken);
        }

    }


}
