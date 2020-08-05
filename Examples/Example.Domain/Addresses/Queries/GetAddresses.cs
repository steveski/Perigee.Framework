namespace Example.Domain.Addresses.Queries
{
    using Example.Domain.Addresses.Views;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddressesBy : IDefineQuery<IEnumerable<GetAddressView>>
    {
        public int? Id { get; set; }

    }

    public class HandleAddressByQuery : IHandleQuery<AddressesBy, IEnumerable<GetAddressView>>
    {
        private readonly IReadEntities _db;

        public HandleAddressByQuery(IReadEntities db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GetAddressView>> Handle(AddressesBy query, CancellationToken cancellationToken)
        {
            var addresses = _db.Query<Address>();

            // Id provided so only use that
            if (query.Id.HasValue)
            {
                var theAddress = await addresses.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken).ConfigureAwait(false);
                if(theAddress == null)
                    return new List<GetAddressView>();

                return new[]
                {
                    new GetAddressView
                    {
                        Id = theAddress.Id,
                        Street = theAddress.Street,
                        Suburb = theAddress.Suburb,
                        PostalCode = theAddress.PostalCode,
                        State = theAddress.State,
                        Country = theAddress.Country
                    }
                };
            }

            // Execute the query and return the results
            var view = await addresses.Select(x => new GetAddressView
            {
                Id = x.Id,
                Street = x.Street,
                Suburb = x.Suburb,
                PostalCode = x.PostalCode,
                State = x.State,
                Country = x.Country
            }).ToListAsync(cancellationToken).ConfigureAwait(false) as IEnumerable<GetAddressView>;

            return view;
        }
    }
}