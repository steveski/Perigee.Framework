namespace Example.Domain.Addresses.Commands
{
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System.Threading;
    using System.Threading.Tasks;

    public class DoSomethingCommand : BaseCreateEntityCommand<Address>, IDefineCommand
    {
        public string DoodadWhatsit { get; set; }
    }

    public class DoSomethingHandler : IHandleCommand<DoSomethingCommand>
    {
        private readonly IWriteEntities _db;

        public DoSomethingHandler(IWriteEntities db)
        {
            _db = db;
        }

        public Task Handle(DoSomethingCommand command, CancellationToken cancellationToken)
        {
            //_db.Create(new Address {Street = command.DoodadWhatsit});

            return Task.CompletedTask;
        }
    }
}