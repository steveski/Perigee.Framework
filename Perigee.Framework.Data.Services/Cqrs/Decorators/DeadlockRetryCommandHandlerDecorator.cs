namespace Perigee.Services.Cqrs.Decorators
{
    using System;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;
    using Perigee.Cqrs.Base.Database;
    using Perigee.Cqrs.Base.Transactions;

    public class DeadlockRetryCommandHandlerDecorator<TCommand> : IHandleCommand<TCommand>
        where TCommand : IDefineCommand
    {
        private readonly IUnitOfWork _db;
        private readonly IHandleCommand<TCommand> _decorated;

        public DeadlockRetryCommandHandlerDecorator(IHandleCommand<TCommand> decorated, IUnitOfWork db)
        {
            _decorated = decorated;
            _db = db;
        }

        public async Task Handle(TCommand command)
        {
            await HandleWithRetry(command, 5);
        }

        private async Task HandleWithRetry(TCommand command, int retries)
        {
            try
            {
                await _decorated.Handle(command);
            }
            catch (Exception ex)
            {
                if (retries <= 0 || !IsDeadlockException(ex))
                    throw;

                Thread.Sleep(300);
                await HandleWithRetry(command, retries - 1);
            }
        }

        private static bool IsDeadlockException(Exception ex)
        {
            return ex is DbException
                   && ex.Message.Contains("deadlock", StringComparison.InvariantCultureIgnoreCase)
                ? true
                : ex.InnerException == null
                    ? false
                    : IsDeadlockException(ex.InnerException);
        }
    }
}