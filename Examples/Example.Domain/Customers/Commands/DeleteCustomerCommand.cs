namespace Example.Domain.Customers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;

    public class DeleteCustomerCommand : BaseEntityCommand
    {
        public int Id { get; set; }


    }

    public class HandleDeleteCustomerCommand : IHandleCommand<DeleteCustomerCommand>
    {
        private readonly IWriteEntities _db;

        public HandleDeleteCustomerCommand(IWriteEntities db)
        {
            _db = db;

        }

        public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            var theCustomer = await _db.Get<Customer>()
                .SingleOrDefaultAsync(c =>c.Id == command.Id, cancellationToken)
                .ConfigureAwait(false);

            if(theCustomer != null)
                _db.Delete(theCustomer);

        }

    }

}
