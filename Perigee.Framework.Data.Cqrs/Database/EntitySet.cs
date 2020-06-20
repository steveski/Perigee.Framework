namespace Perigee.Cqrs.Base.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Entities;

    public class EntitySet<TEntity> : IQueryable<TEntity> where TEntity : Entity
    {
        public EntitySet(IQueryable<TEntity> queryable, IReadEntities entities)
        {
            Queryable = queryable ?? throw new ArgumentNullException(nameof(queryable));
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }

        internal IQueryable<TEntity> Queryable { get; set; }
        internal IReadEntities Entities { get; }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression => Queryable.Expression;

        public Type ElementType => Queryable.ElementType;

        public IQueryProvider Provider => Queryable.Provider;
    }
}