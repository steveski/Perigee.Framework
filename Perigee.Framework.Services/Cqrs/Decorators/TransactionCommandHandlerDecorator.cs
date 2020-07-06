namespace Perigee.Framework.Services.Cqrs.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;

    public class TransactionCommandHandlerDecorator<TCommand> : IHandleCommand<TCommand>
        where TCommand : BaseEntityCommand
    {
        private readonly IHandleCommand<TCommand> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionCommandHandlerDecorator(IHandleCommand<TCommand> decorated, IUnitOfWork unitOfWork)
        {
            this._decorated = decorated;
            this._unitOfWork = unitOfWork;
        }


        public async Task Handle(TCommand command, CancellationToken cancellationToken)
        {
            if (command.Commit)
            {
                using var scope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled);

                await _decorated.Handle(command, cancellationToken).ConfigureAwait(false);
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                scope.Complete();

            }
            else
            {
                await _decorated.Handle(command, cancellationToken).ConfigureAwait(false);

            }



        }
    }
}