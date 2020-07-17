namespace Perigee.Framework.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using EnsureThat;
    using LinqKit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using ModelCreation;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Entities;
    using EntityState = Perigee.Framework.Base.Database.EntityState;
    using EfEntityState = Microsoft.EntityFrameworkCore.EntityState;


    public class EntityDbContext : DbContext, IWriteEntities
    {
        private readonly IRecordAuthority _recordAuthority;
        private readonly IAuditedEntityUpdater _auditedEntityUpdater;

        public EntityDbContext(DbContextOptions<EntityDbContext> options, IRecordAuthority recordAuthority, IAuditedEntityUpdater auditedEntityUpdater) : base(options)
        {
            Ensure.Any.IsNotNull(recordAuthority, nameof(recordAuthority));
            Ensure.Any.IsNotNull(auditedEntityUpdater, nameof(auditedEntityUpdater));

            _recordAuthority = recordAuthority;
            _auditedEntityUpdater = auditedEntityUpdater;
            
        }

        private void SetAuditValues()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EfEntityState.Added)
                .Select(x => x.Entity);

            var updatedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EfEntityState.Modified)
                .Select(x => x.Entity);

            _auditedEntityUpdater.UpdateAuditFields(addedEntities, updatedEntities);

        }

        private void SetSoftDelete(EntityEntry entry)
        {
            entry.State = EfEntityState.Modified;
            ((ISoftDelete) entry.Entity).IsDeleted = true;
        }

        #region Model Creation

        public ICreateDbModel ModelCreator { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreator ??= new DefaultDbModelCreator();
            ModelCreator.Create(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Queries

        public new IQueryable<TEntity> Query<TEntity>() where TEntity : class, IEntity
        {
            // AsNoTracking returns entities that are not attached to the DbContext
            return QueryUnfiltered<TEntity>().Where(_recordAuthority.Clause<TEntity>());
        }

        public IQueryable<TEntity> Query<TEntity>(bool includeSoftDeleted) where TEntity : class, IEntity, ISoftDelete
        {
            var query = Query<TEntity>();
            if (includeSoftDeleted)
                query = query.Where(x => x.IsDeleted);

            return query;
        }

        public IQueryable<TEntity> QueryUnfiltered<TEntity>() where TEntity : class, IEntity
        {
            // AsNoTracking returns entities that are not attached to the DbContext
            return Set<TEntity>().AsNoTracking().AsExpandable();
        }

        public IQueryable<TEntity> QueryUnfiltered<TEntity>(bool includeSoftDeleted) where TEntity : class, IEntity, ISoftDelete
        {
            var query = QueryUnfiltered<TEntity>();
            if (includeSoftDeleted)
                query = query.Where(x => x.IsDeleted);

            return query;
        }

        #endregion

        #region Commands

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class, IEntity
        {
            return Set<TEntity>().AsExpandable().Where(_recordAuthority.Clause<TEntity>());
        }

        public IQueryable<TEntity> Get<TEntity>(bool includeSoftDeleted) where TEntity : class, IEntity, ISoftDelete
        {
            var query = Get<TEntity>();
            if (includeSoftDeleted)
                query = query.Where(x => x.IsDeleted);

            return query;
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (Entry(entity).State == EfEntityState.Detached) Set<TEntity>().Add(entity);
        }

        public new void Update<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var entry = Entry(entity);
            if (entry.State != EfEntityState.Added)
                entry.State = EfEntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (Entry(entity).State != EfEntityState.Deleted)
                Set<TEntity>().Remove(entity);
        }

        public void Reload<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            Entry(entity).Reload();
        }

        public Task ReloadAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IEntity
        {
            return Entry(entity).ReloadAsync(cancellationToken);
        }

        public new void Attach<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            if (Entry(entity).State == EfEntityState.Detached)
                Set<TEntity>().Attach(entity);
        }

        public EntityState GetState<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var internalEntityState = MapToInternal(Entry(entity).State);
            return internalEntityState;
        }
        
        public void SetEntityState<TEntity>(TEntity entity, EntityState state) where TEntity : class, IEntity
        {
            Entry(entity).State = MapToEf(state);
        }

        private EfEntityState MapToEf(EntityState state)
        {
            var efEntityState = (EfEntityState)Enum.Parse(typeof(EfEntityState), state.ToString());
            return efEntityState;
        }

        private EntityState MapToInternal(EfEntityState state)
        {
            var internalState = (EntityState)Enum.Parse(typeof(EntityState), state.ToString());
            return internalState;
        }

        #endregion

        #region UnitOfWork

        public void DiscardChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x != null))
                switch (entry.State)
                {
                    case EfEntityState.Added:
                        entry.State = EfEntityState.Detached;
                        break;
                    case EfEntityState.Modified:
                        entry.State = EfEntityState.Unchanged;
                        break;
                    case EfEntityState.Deleted:
                        entry.Reload();
                        break;
                }
        }

        public Task DiscardChangesAsync(CancellationToken cancellationToken)
        {
            var reloadTasks = new List<Task>();
            foreach (var entry in ChangeTracker.Entries().Where(x => x != null))
                switch (entry.State)
                {
                    case EfEntityState.Added:
                        entry.State = EfEntityState.Detached;
                        break;
                    case EfEntityState.Modified:
                        entry.State = EfEntityState.Unchanged;
                        break;
                    case EfEntityState.Deleted:
                        reloadTasks.Add(entry.ReloadAsync(cancellationToken));
                        break;
                }

            return Task.WhenAll(reloadTasks);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var softDeletedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EfEntityState.Deleted && x.Entity is ISoftDelete);
            
            softDeletedEntities.ToList().ForEach(SetSoftDelete);

            SetAuditValues();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync(CancellationToken.None).Result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        #endregion
    }
}
