namespace Example.Domain.Customers.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
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
        private readonly IMapper _mapper;

        public HandleCustomersWithIncludeByQuery(IReadEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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
                var custView = _mapper.Map<Customer, GetCustomerWithAddressView>(theCustomer);

                return new[]
                {
                    custView
                };
            }

            // Apply filters
            if (!string.IsNullOrEmpty(query.FirstName))
                customersQuery = customersQuery.Where(x => x.FirstName.Contains(query.FirstName));

            if (!string.IsNullOrEmpty(query.LastName))
                customersQuery = customersQuery.Where(x => x.LastName.Contains(query.LastName));

            // Execute the query and return the results
            var customers = await customersQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

            // Return the results
            return _mapper.Map<IEnumerable<Customer>, IEnumerable<GetCustomerWithAddressView>>(customers);
        }
    }
}