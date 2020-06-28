namespace Perigee.Framework.Services.Cqrs.Decorators
{
    using System.Threading;
    using System.Threading.Tasks;
    using Perigee.Framework.Cqrs.Database;
    using Perigee.Framework.Cqrs.Transactions;

    public class TransactionCommandHandlerDecorator<TCommand> : IHandleCommand<TCommand>
        where TCommand : IDefineCommand
    {
        private readonly IHandleCommand<TCommand> decorated;
        private readonly IUnitOfWork unitOfWork;

        public TransactionCommandHandlerDecorator(IHandleCommand<TCommand> decorated, IUnitOfWork unitOfWork)
        {
            this.decorated = decorated;
            this.unitOfWork = unitOfWork;
        }


        public async Task Handle(TCommand command, CancellationToken cancellationToken)
        {
            // TODO: Cleanup. This was a problem at one point due to be not being sure about the transparent transactions issue.
            //using (unitOfWork)
            //{
            await decorated.Handle(command, cancellationToken).ConfigureAwait(false);
            var killIt = false;
            if (killIt)
                await unitOfWork.DiscardChangesAsync(cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            //}
        }
    }
}