namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;

    public abstract class BaseQueryViews<TEntity> : BaseQuery<IEnumerable<TEntity>>
    {
        public IDictionary<Expression<Func<TEntity, object>>, OrderByDirection> OrderBy { get; set; }
        public string Sort { get; set; } //OrderByProperty
        public string Order { get; set; } //OrderByDirection
    }
}