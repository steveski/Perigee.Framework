namespace Perigee.Cqrs.Base.Transactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Entities;

    public abstract class BaseQueryEntity<TEntity> : BaseQuery<TEntity> where TEntity : Entity
    {
        public virtual IEnumerable<Expression<Func<TEntity, object>>> EagerLoad { get; set; }
    }
}