namespace Example.Domain.Customers.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Example.Entities;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Views;

    public class CustomersBy : IDefineQuery<IEnumerable<GetCustomerWithAddressView>>
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class HandleCustomerByQuery : IHandleQuery<CustomersBy, IEnumerable<GetCustomerWithAddressView>>
    {
        private readonly IReadEntities _db;
        private readonly IMapper _mapper;

        public HandleCustomerByQuery(IReadEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCustomerWithAddressView>> Handle(CustomersBy query, CancellationToken cancellationToken)
        {
            var customers = _db.Query<Customer>();

            // Id provided so only use that
            if (query.Id.HasValue)
            {
                var theCustomer = await customers.Select(GetCustomerWithAddressView.Projector).FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken).ConfigureAwait(false);
                if(theCustomer == null)
                    return new List<GetCustomerWithAddressView>();

                var custView = _mapper.Map<Customer, GetCustomerView>(theCustomer);

                return new[]
                {
                    custView
                };
            }

            // Apply filters
            if (!string.IsNullOrEmpty(query.FirstName))
                customers = customers.Where(x => x.FirstName.Contains(query.FirstName));

            if (!string.IsNullOrEmpty(query.LastName))
                customers = customers.Where(x => x.LastName.Contains(query.LastName));

            // Return the results
            return _mapper.Map<IEnumerable<Customer>, IEnumerable<GetCustomerView>>(customers);
        }
    }
}