using AutoMapper;
using Example.Domain.CustomerEmployerMappings.Views;
using Example.Entities;
using Microsoft.EntityFrameworkCore;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Transactions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Domain.CustomerEmployerMappings.Queries
{
    public class GetCustomerEmployerMappings : IDefineQuery<IEnumerable<GetCustomerEmployerMappingDetailedDataView>>
    {
    }

    public class HandleCustomersWithIncludeByQuery : IHandleQuery<GetCustomerEmployerMappings, IEnumerable<GetCustomerEmployerMappingDetailedDataView>>
    {
        private readonly IReadEntities _db;
        private readonly IMapper _mapper;

        public HandleCustomersWithIncludeByQuery(IReadEntities db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCustomerEmployerMappingDetailedDataView>> Handle(GetCustomerEmployerMappings query, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<CustomerEmployerMapping, object>>>
            {
                cem => cem.Customer,
                cem => cem.Employer,
            };

            var cemQuery = _db.Query(includes);

            // Equivalent to:
            //var cemQuery = _db.Query<CustomerEmployerMapping>().Include(cem => cem.Customer).Include(cem => cem.Employer);

            // Execute the query and return the results
            var cems = await cemQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

            // Return the results
            return _mapper.Map<IEnumerable<CustomerEmployerMapping>, IEnumerable<GetCustomerEmployerMappingDetailedDataView>>(cems);
        }
    }
}
