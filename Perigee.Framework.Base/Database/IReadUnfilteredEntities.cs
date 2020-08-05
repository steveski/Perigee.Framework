namespace Perigee.Framework.Base.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Perigee.Framework.Base.Entities;

    public interface IReadUnfilteredEntities
    {
        IQueryable<TEntity> QueryUnfiltered<TEntity>() where TEntity : class, IEntity;
        IQueryable<TEntity> QueryUnfiltered<TEntity>(bool includeSoftDeleted) where TEntity : class, IEntity, ISoftDelete;
        
        IQueryable<TEntity> QueryUnfiltered<TEntity, TProperty>(IEnumerable<Expression<Func<TEntity, TProperty>>> includes) where TEntity : class, IEntity;
        IQueryable<TEntity> QueryUnfiltered<TEntity, TProperty>(IEnumerable<Expression<Func<TEntity, TProperty>>> includes, bool includeSoftDeleted) where TEntity : class, IEntity, ISoftDelete;

    }
}