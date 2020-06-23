namespace Example.Domain.Customers.Commands
{
    using System.Threading.Tasks;
    using Entities;
    using Perigee.Framework.Data.Cqrs.Database;
    using Perigee.Framework.Data.Cqrs.Transactions;

    public class DoSomethingCommand : BaseCreateEntityCommand<Customer>, IDefineCommand
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

        public async Task Handle(DoSomethingCommand command)
        {
            //_db.Create(new Customer {FirstName = command.DoodadWhatsit});
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}