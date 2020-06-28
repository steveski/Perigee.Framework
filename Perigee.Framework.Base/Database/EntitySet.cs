namespace Perigee.Framework.Base.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using EnsureThat;
    using Entities;

    public class EntitySet<TEntity> : IQueryable<TEntity> where TEntity : Entity
    {
        public EntitySet(IQueryable<TEntity> queryable, IReadEntities entities)
        {
            Ensure.Any.IsNotNull(queryable, nameof(queryable));
            Ensure.Any.IsNotNull(entities, nameof(entities));

            Queryable = queryable;
            Entities = entities;

        }

        internal IQueryable<TEntity> Queryable { get; set; }
        internal IReadEntities Entities { get; }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Queryable.GetEnumerator();
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Expression Expression => Queryable.Expression;

        public Type ElementType => Queryable.ElementType;

        public IQueryProvider Provider => Queryable.Provider;
    }
}