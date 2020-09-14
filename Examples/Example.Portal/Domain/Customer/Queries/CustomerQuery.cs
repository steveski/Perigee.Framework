using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Portal.Domain.Customer.Queries
{
    public class CustomerQuery : IDefineQuery<IEnumerable<Entities.Customer>>
    {
        public Guid? id { get; set; }
    }

    public class HandleCustomerQuery : IHandleQuery<CustomerQuery, IEnumerable<Entities.Customer>>
    {
        private readonly IReadEntities _db;

        public HandleCustomerQuery(IReadEntities db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Entities.Customer>> Handle(CustomerQuery query, CancellationToken cancellationToken)
        {
            var customers = _db.Query<Entities.Customer>();
            if (query.id.HasValue)
                customers = customers.Where(c => c.Id == query.id);

            return await Task.FromResult(customers);
        }
    }
}
