namespace Example.Domain.Addresses.Queries
{
    using AutoMapper;
    using Example.Domain.Addresses.Views;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddressesBy : IDefineQuery<IEnumerable<GetAddressView>>
    {
        public int? Id { get; set; }

    }

    public class HandleAddressByQuery : IHandleQuery<AddressesBy, IEnumerable<GetAddressView>>
    {
        private readonly IReadEntities _db;
        private readonly IMapper _mapper;

        public HandleAddressByQuery(IReadEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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

                var addressView = _mapper.Map<Address, GetAddressView>(theAddress);

                return new[]
                {
                    addressView
                };
            }

            // Return the results
            return _mapper.Map<IEnumerable<Address>, IEnumerable<GetAddressView>>(addresses);
        }
    }
}