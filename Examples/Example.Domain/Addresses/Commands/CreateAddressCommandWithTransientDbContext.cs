namespace Example.Domain.Addresses.Commands
{
    using Entities;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateAddressCommandWithTransientDbContext : BaseCommand, IDefineCommand
    {
        //public Guid Id { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public Address Address { get; internal set; }

    }

    public class HandleCreateAddressCommandWithTransientDbContext : IHandleCommand<CreateAddressCommandWithTransientDbContext>
    {
        private readonly ITransientContext _db;

        public HandleCreateAddressCommandWithTransientDbContext(ITransientContext db)
        {
            _db = db;
        }

        public async Task Handle(CreateAddressCommandWithTransientDbContext command, CancellationToken cancellationToken)
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
            command.Address = address;
            
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    //public class SomeService
    //{
    //    private BlockingCollection<int> messageQueue = new BlockingCollection<int>();

    //    public void ProcessIncommingMessages(CancellationToken ct)
    //    {
    //        while (true)
    //        {
    //            var message = messageQueue.Take(ct);
    //            Console.Write(message);

    //        }
    //    }

    //    public void SubmitMessageForProcessing(int message)
    //    {
    //        messageQueue.Add(message);
    //    }

    //}
}