namespace Perigee.Framework.Base.Database
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    /// <summary>
    ///     Informs an underlying relational data store to accept or return sets of writeable entity instances.
    /// </summary>
    public interface IWriteEntities : IUnitOfWork, IReadEntities, IReadUnfilteredEntities
    {
        /// <summary>
        ///     Inform an underlying relational data store to return a set of writable entity instances.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instances that the underlying relational data store
        ///     should return.
        /// </typeparam>
        /// <returns>
        ///     IQueryable for set of writable TEntity instances from an underlying relational data
        ///     store.
        /// </returns>
        IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        ///     Inform the underlying relational data store that a new entity instance should be added to a set
        ///     of entity instances.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instance set that the underlying relational data
        ///     store should add to the entity instance to.
        /// </typeparam>
        /// <param name="entity">
        ///     Entity instance that should be added to the TEntity set by the underlying
        ///     relational data store.
        /// </param>
        void Create<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     Inform the underlying relational data store that an existing entity instance should be
        ///     permanently removed from its set of entity instances.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instance set that the underlying relational data
        ///     store should permanently remove the entity instance from.
        /// </typeparam>
        /// <param name="entity">
        ///     Entity instance that should be permanently removed from the TEntity set by
        ///     the underlying relational data store.
        /// </param>
        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     Inform the underlying relational data store that an existing entity instance's data state may
        ///     have changed.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instance set that the changed entity instance
        ///     is part of.
        /// </typeparam>
        /// <param name="entity">
        ///     Entity instance whose data state may be different from that of the
        ///     underlying relational data store.
        /// </param>
        void Update<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     Inform the underlying relational data store to replace the data state of an entity instance with
        ///     the values currently saved in the underlying relational data store.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instance set that the entity instance to be reloaded
        ///     is part of.
        /// </typeparam>
        /// <param name="entity">
        ///     Entity instance whose data state will be replaced using the values currently
        ///     saved in the underlying relational data store.
        /// </param>
        void Reload<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     Asynchronously inform the underlying relational data store to replace the data state of an entity instance with
        ///     the values currently saved in the underlying relational data store.
        /// </summary>
        /// <typeparam name="TEntity">
        ///     Type of the entity instance set that the entity instance to be reloaded
        ///     is part of.
        /// </typeparam>
        /// <param name="entity">
        ///     Entity instance whose data state will be replaced using the values currently
        ///     saved in the underlying relational data store.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ReloadAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IEntity;

        /// <summary>
        ///     Re-attach a detached entity, i.e. one loaded with Query (IReadEntities) including entities returned from
        ///     IProcess/HandleQueries
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        ///     returns the entity's state within the context
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityState GetState<TEntity>(TEntity entity) where TEntity : class, IEntity;

        /// <summary>
        /// Returns the Entry for the specified entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        void SetEntityState<TEntity>(TEntity entity, EntityState state) where TEntity : class, IEntity;
        
    }
}
