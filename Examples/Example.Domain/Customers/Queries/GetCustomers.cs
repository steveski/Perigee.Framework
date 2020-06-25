namespace Example.Domain.Customers.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Data.Cqrs.Database;
    using Perigee.Framework.Data.Cqrs.Transactions;
    using Perigee.Framework.Data.Helpers.Shared;
    using Views;

    public class CustomersBy : IDefineQuery<IEnumerable<GetCustomerView>>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [UsedImplicitly]
    public class HandleCustomerByQuery : IHandleQuery<CustomersBy, IEnumerable<GetCustomerView>>
    {
        private readonly IReadEntities _db;

        public HandleCustomerByQuery(IReadEntities db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GetCustomerView>> Handle(CustomersBy query)
        {
            var customers = _db.Query<Customer>();

            // Id provided so only use that
            if (query.Id.HasValue)
            {
                var theCustomer = await customers.SingleOrDefaultAsync(x => x.Id == query.Id).ConfigureAwait(false);
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

            // Apply filters
            if (!string.IsNullOrEmpty(query.FirstName))
                customers = customers.Where(x => x.FirstName.Contains(query.FirstName));

            if (!string.IsNullOrEmpty(query.LastName))
                customers = customers.Where(x => x.LastName.Contains(query.LastName));

            // Execute the query and return the results
            var view = await customers.Select(x => new GetCustomerView
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress
            }).ToListAsync().ConfigureAwait(false) as IEnumerable<GetCustomerView>;

            return view;
        }
    }
}