namespace Example.Domain.Customers.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;

    public class CreateCustomerCommand : BaseCreateEntityCommand<Customer>, IDefineCommand
    {
        //public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class HandleCreateCustomerCommand : IHandleCommand<CreateCustomerCommand>
    {
        private readonly ITransientContext _db;

        public HandleCreateCustomerCommand(ITransientContext db)
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