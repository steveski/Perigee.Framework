namespace Example.Domain.Customers.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;

    public class CreateCustomerCommand : BaseCreateEntityCommand<Customer>
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

        public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var cust = new Customer
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress
            };

            _db.Create(cust);
            command.CreatedEntity = cust;
            
        }
    }

    public class SomeService
    {
        private BlockingCollection<int> messageQueue = new BlockingCollection<int>();

        public void ProcessIncommingMessages(CancellationToken ct)
        {
            while (true)
            {
                var message = messageQueue.Take(ct);
                Console.Write(message);

            }
        }

        public void SubmitMessageForProcessing(int message)
        {
            messageQueue.Add(message);
        }

    }
}