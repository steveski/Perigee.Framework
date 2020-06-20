namespace Perigee.Framework.Data.Services.Cqrs.Decorators
{
    using System.Threading.Tasks;
    using Data.Cqrs.Database;
    using Data.Cqrs.Transactions;

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


        public async Task Handle(TCommand command)
        {
            //using (unitOfWork)
            //{
            await decorated.Handle(command);
            var killIt = false;
            if (killIt)
                await unitOfWork.DiscardChangesAsync();
            await unitOfWork.SaveChangesAsync();

            //}
        }
    }
}