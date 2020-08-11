namespace Perigee.Framework.EntityFramework.IntegrationTests.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;

    public class PersonByQuery : IDefineQuery<IEnumerable<PersonByView>>
    {
        public bool IncludeDeleted { get; set; }
        public string Name { get; set; }

    }


    public class HandlePersonByQuery : IHandleQuery<PersonByQuery, IEnumerable<PersonByView>>
    {
        private readonly IReadEntities _db;

        public HandlePersonByQuery(IReadEntities db)
        {
            _db = db;

        }

        public async Task<IEnumerable<PersonByView>> Handle(PersonByQuery query, CancellationToken cancellationToken)
        {
            var personQuery = _db.Query<Person>(query.IncludeDeleted);

            if(string.IsNullOrWhiteSpace(query.Name) == false)
                personQuery = personQuery.Where(p => p.Name == query.Name);

            return await personQuery.Select(PersonByView.Projector).ToListAsync(cancellationToken);
        }

    }

    public class PersonByView
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Description { get; set; }

        public static Expression<Func<Person, PersonByView>> Projector => p => new PersonByView
        {
            Name = p.Name,
            Age = p.Age,
            Description = p.Description

        };

    }
}
