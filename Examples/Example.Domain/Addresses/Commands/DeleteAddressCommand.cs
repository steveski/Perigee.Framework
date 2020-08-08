namespace Example.Domain.Addresses.Commands
{
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteAddressCommand : BaseEntityCommand
    {
        public int Id { get; set; }


    }

    public class HandleDeleteAddressCommand : IHandleCommand<DeleteAddressCommand>
    {
        private readonly IWriteEntities _db;

        public HandleDeleteAddressCommand(IWriteEntities db)
        {
            _db = db;

        }

        public async Task Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
        {
            var theAddress = await _db.Get<Address>()
                .SingleOrDefaultAsync(a =>a.Id == command.Id, cancellationToken)
                .ConfigureAwait(false);

            if(theAddress != null)
                _db.Delete(theAddress);

        }

    }

}
