namespace Example.Domain.Customers.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using EnsureThat;
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;

    public class CreateCustomerCommand : BaseCreateEntityCommand<Customer>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        
    }

    public class HandleCreateCustomerCommand : IHandleCommand<CreateCustomerCommand>
    {
        private readonly IWriteEntities _db;
        private readonly IUserService _userService;

        public HandleCreateCustomerCommand(IWriteEntities db, IUserService userService)
        {
            Ensure.Any.IsNotNull(db, nameof(db));
            Ensure.Any.IsNotNull(userService, nameof(userService));

            _db = db;
            _userService = userService;
        }

        public Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var cust = new Customer
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                EmailAddress = command.EmailAddress,
                ManagedBy = _userService.ClaimsIdentity.Name
            };

            _db.Create(cust);
            command.CreatedEntity = cust;
            
            return Task.CompletedTask;
        }
    }

    public class SomeService
    {
        private readonly BlockingCollection<int> _messageQueue = new BlockingCollection<int>();

        public void ProcessIncomingMessages(CancellationToken ct)
        {
            while (true)
            {
                var message = _messageQueue.Take(ct);
                Console.Write(message);

            }
        }

        public void SubmitMessageForProcessing(int message)
        {
            _messageQueue.Add(message);
        }

    }
}