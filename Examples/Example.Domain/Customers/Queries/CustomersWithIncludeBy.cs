namespace Example.Domain.Customers.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Example.Domain.Addresses.Views;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Views;

    public class CustomersWithIncludeBy : IDefineQuery<IEnumerable<GetCustomerWithAddressView>>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class HandleCustomersWithIncludeByQuery : IHandleQuery<CustomersWithIncludeBy, IEnumerable<GetCustomerWithAddressView>>
    {
        private readonly IReadEntities _db;

        public HandleCustomersWithIncludeByQuery(IReadEntities db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GetCustomerWithAddressView>> Handle(CustomersWithIncludeBy query, CancellationToken cancellationToken)
        {
            // Create a list of the expressions you would usually put into Include() statements.
            // For example if you want .Include(c => Address), do this...
            var includes = new List<Expression<Func<Customer, object>>>
            {
                c => c.Address,
                //c => c.OtherRelatedEntity
            };

            // Pass include expressions list to Query(). When providing this list it's not necessary to specify generic types 
            // to specify generic types as they can be inferred from the List instance.
            var customersQuery = _db.Query(includes);

            // Equivalent to:
            //var customersQuery = _db.Query<Customer>().Include(c => c.Address);

            // Id provided so only use that
            if (query.Id.HasValue)
            {
                var theCustomer = await customersQuery.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken).ConfigureAwait(false);
                if(theCustomer == null)
                    return new List<GetCustomerWithAddressView>();

                return new[]
                {
                    new GetCustomerWithAddressView
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
                customersQuery = customersQuery.Where(x => x.FirstName.Contains(query.FirstName));

            if (!string.IsNullOrEmpty(query.LastName))
                customersQuery = customersQuery.Where(x => x.LastName.Contains(query.LastName));

            // Execute the query and return the results
            var customers = await customersQuery.ToListAsync(cancellationToken).ConfigureAwait(false);
            var view = customers.Select(cust => new GetCustomerWithAddressView
            {
                Id = cust.Id,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                EmailAddress = cust.EmailAddress,

                Address = cust.Address == null ? null : new AddressView
                {
                    Id = cust.Address.Id,
                    Street = cust.Address.Street,
                    Suburb = cust.Address.Suburb,
                    PostalCode = cust.Address.PostalCode,
                    State = cust.Address.State,
                    Country = cust.Address.Country

                }
            });

            return view;
        }
    }
}