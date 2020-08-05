namespace Example.Domain.Addresses.Commands
{
    using EnsureThat;
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateAddressCommand : BaseCreateEntityCommand<Address>
    {
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class HandleCreateAddressCommand : IHandleCommand<CreateAddressCommand>
    {
        private readonly IWriteEntities _db;

        public HandleCreateAddressCommand(IWriteEntities db)
        {
            Ensure.Any.IsNotNull(db, nameof(db));

            _db = db;
        }

        public Task Handle(CreateAddressCommand command, CancellationToken cancellationToken)
        {
            var address = new Address
            {
                Street = command.Street,
                Suburb = command.Suburb,
                PostalCode = command.PostalCode,
                State = command.State,
                Country = command.Country,
            };

            _db.Create(address);
            command.CreatedEntity = address;
            
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