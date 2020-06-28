namespace Example.Domain.Customers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Perigee.Framework.Cqrs.Database;
    using Perigee.Framework.Cqrs.Transactions;

    public class CreateCustomerCommand : BaseCreateEntityCommand<Customer>, IDefineCommand
    {
        //public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class HandleCreateCustomerCommand : IHandleCommand<CreateCustomerCommand>
    {
        private readonly IWriteEntities _db;

        public HandleCreateCustomerCommand(IWriteEntities db)
        {
            _db = db;
        }

        public Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var cust = new Customer
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress
            };

            _db.Create(cust);
            command.CreatedEntity = cust;

            return Task.CompletedTask;
        }
    }
}