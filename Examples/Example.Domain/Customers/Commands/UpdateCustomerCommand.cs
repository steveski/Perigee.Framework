using Example.Entities;
using Microsoft.EntityFrameworkCore;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Domain.Customers.Commands
{
    public class UpdateCustomerCommand : BaseEntityCommand
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class HandleUpdateCustomerCommand : IHandleCommand<UpdateCustomerCommand>
    {
        private readonly IWriteEntities _db;

        public HandleUpdateCustomerCommand(IWriteEntities db)
        {
            _db = db;
        }

        public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _db.Query<Customer>().FirstOrDefaultAsync(c => c.Id == command.Id);

            if (customer != null)
            {
                customer.FirstName = command.FirstName;
                customer.LastName = command.LastName;
                customer.EmailAddress = command.EmailAddress;

                _db.Update(customer);
            }
        }
    }
}
