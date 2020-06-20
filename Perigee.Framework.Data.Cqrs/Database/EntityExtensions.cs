namespace Perigee.Framework.Data.Cqrs.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Entities;

    public static class EntityExtensions
    {
        #region EagerLoad

        public static IQueryable<TEntity> EagerLoad<TEntity>(this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, object>> expression)
            where TEntity : Entity
        {
            var set = queryable as EntitySet<TEntity>;
            if (set != null)
                set.Queryable = set.Entities.EagerLoad(set.Queryable, expression);
            return queryable;
        }

        public static IQueryable<TEntity> EagerLoad<TEntity>(this IQueryable<TEntity> queryable,
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
            where TEntity : Entity
        {
            if (expressions != null)
                queryable = expressions.Aggregate(queryable, (current, expression) => current.EagerLoad(expression));
            return queryable;
        }

        #endregion
    }
}