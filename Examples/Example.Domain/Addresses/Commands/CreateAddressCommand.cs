namespace Example.Domain.Addresses.Commands
{
    using AutoMapper;
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
        private readonly IMapper _mapper;

        public HandleCreateAddressCommand(IWriteEntities db, IMapper mapper)
        {
            Ensure.Any.IsNotNull(db, nameof(db));

            _db = db;
            _mapper = mapper;
        }

        public Task Handle(CreateAddressCommand command, CancellationToken cancellationToken)
        {
            var address = _mapper.Map<CreateAddressCommand, Address>(command);

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