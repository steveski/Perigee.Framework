namespace Perigee.Framework.Base.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;

    public abstract class BaseQueryEntities<TEntity> : BaseQuery<IEnumerable<TEntity>> where TEntity : Entity
    {
        public IEnumerable<Expression<Func<TEntity, object>>> EagerLoad { get; set; }
        public IDictionary<Expression<Func<TEntity, object>>, OrderByDirection> OrderBy { get; set; }

        // decision that returning a list of entities should not paginate - if you want to paginate, return a list of views....
        //public PagingInformation PagingInformation { get; set; }
    }
}