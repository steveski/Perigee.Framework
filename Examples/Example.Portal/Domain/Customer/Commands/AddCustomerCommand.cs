using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Portal.Domain.Customer.Commands
{
    public class AddCustomerCommand : BaseCreateEntityCommand<Entities.Customer>
    {
        public string customerName { get; set; }
    }

    public class HandleAddCustomerCommand : IHandleCommand<AddCustomerCommand>
    {
        private readonly IWriteEntities _db;

        public HandleAddCustomerCommand(IWriteEntities db)
        {
            _db = db;
        }

        public Task Handle(AddCustomerCommand command, CancellationToken cancellationToken)
        {
            var newCust = new Entities.Customer
            {
                customerName = command.customerName
            };

            _db.Create(newCust);
            command.CreatedEntity = newCust;

            return Task.CompletedTask;
        }
    }
}
