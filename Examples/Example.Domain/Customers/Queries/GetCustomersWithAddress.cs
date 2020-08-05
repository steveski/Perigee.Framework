#define USE_AUTOMAPPER

using AutoMapper;
using Example.Domain.Customers.Views;
using Example.Entities;
using Microsoft.EntityFrameworkCore;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Domain.Customers.Queries
{
    public class CustomersWithAddressBy : IDefineQuery<IEnumerable<GetCustomerWithAddressView>>
    {
//        public int? Id { get; set; }
        public string FirstName { get; set; }
//        public string LastName { get; set; }
    }

    public class HandleCustomerWithAddressByQuery : IHandleQuery<CustomersWithAddressBy, IEnumerable<GetCustomerWithAddressView>>
    {
        private readonly IReadEntities _db;
        private readonly IMapper _mapper;

        public HandleCustomerWithAddressByQuery(IReadEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCustomerWithAddressView>> Handle(CustomersWithAddressBy query, CancellationToken cancellationToken)
        {
            var customersWithAddress = _db.Query<Customer>();

            /*
            // Id provided so only use that
            if (query.Id.HasValue)
            {
                var theCustomer = await customers.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken).ConfigureAwait(false);
                if (theCustomer == null)
                    return new List<GetCustomerView>();

                return new[]
                {
                    new GetCustomerView
                    {
                        Id = theCustomer.Id,
                        FirstName = theCustomer.FirstName,
                        LastName = theCustomer.LastName,
                        EmailAddress = theCustomer.EmailAddress
                    }
                };
            }

*/
            // Apply filters
            if (!string.IsNullOrEmpty(query.FirstName))
                customersWithAddress = customersWithAddress.Where(x => x.FirstName.Contains(query.FirstName));

            /*
            if (!string.IsNullOrEmpty(query.LastName))
                customers = customers.Where(x => x.LastName.Contains(query.LastName));
*/

            customersWithAddress = customersWithAddress.Include(c => c.Address);

#if USE_AUTOMAPPER
            // Return the results
            return _mapper.Map<IEnumerable<Customer>, IEnumerable<GetCustomerWithAddressView>>(customersWithAddress);
#else
            // This code does do the righ thing, but I don't want to use it as it doesn't use AutoMapper,
            // and gets annoying / risky when adding a member variable.
            // Execute the query and return the results
            var view = await customersWithAddress.Select(x => new GetCustomerWithAddressView
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                Street = x.Address.Street,
                Suburb = x.Address.Suburb,
                PostalCode = x.Address.PostalCode,
                State = x.Address.State,
                Country = x.Address.Country
            }).ToListAsync(cancellationToken).ConfigureAwait(false) as IEnumerable<GetCustomerWithAddressView>;

            return view;
#endif // USE_AUTOMAPPER
        }
    }
}
