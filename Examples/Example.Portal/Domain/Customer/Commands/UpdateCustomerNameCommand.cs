using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Portal.Domain.Customer.Commands
{
    public class UpdateCustomerNameCommand : BaseEntityCommand
    {
        public Guid id { get; set; }
        public string customerName { get; set; }
    }

    public class HandleUpdateCustomerNameCommand : IHandleCommand<UpdateCustomerNameCommand>
    {
        private readonly IWriteEntities _db;

        public HandleUpdateCustomerNameCommand(IWriteEntities db)
        {
            _db = db;
        }

        public Task Handle(UpdateCustomerNameCommand command, CancellationToken cancellationToken)
        {
            var entity = _db.Get<Entities.Customer>().FirstOrDefault(x => x.Id == command.id);
            entity.customerName = command.customerName;

            return Task.CompletedTask;
        }
    }
}
