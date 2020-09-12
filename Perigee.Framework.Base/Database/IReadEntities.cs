namespace Perigee.Framework.Base.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Entities;

    /// <summary>
    ///     Informs an underlying relational data store to return sets of read-only entity instances.
    /// </summary>
    public interface IReadEntities
    {
        /// <summary>
        ///     Inform an underlying relational data store to return a set of read-only entity instances.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instances that the underlying relational data
        ///     store should return.
        /// </typeparam>
        /// <returns>
        ///     IQueryable for set of read-only TEntity instances from an underlying relational
        ///     data store.
        /// </returns>
        IQueryable<TEntity> Query<TEntity>(bool includeSoftDeleted = false) where TEntity : class, IEntity;
        
        IQueryable<TEntity> Query<TEntity, TProperty>(IEnumerable<Expression<Func<TEntity, TProperty>>> includes, bool includeSoftDeleted = false) where TEntity : class, IEntity;
        

    }
}