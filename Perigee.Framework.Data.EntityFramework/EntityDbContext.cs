﻿namespace Perigee.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Cqrs.Base.Database;
    using Cqrs.Base.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using ModelCreation;

    public class EntityDbContext : DbContext, IWriteEntities
    {
        #region Construction & Initialization

        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {
            ////Initializer = new BrownfieldDbInitialiser();
            /// 
        }

        ////private IDatabaseInitialiser<EntityDbContext> _initializer;

        ////public IDatabaseInitializer<EntityDbContext> Initializer
        ////{
        ////    get { return _initializer; }
        ////    set
        ////    {
        ////        _initializer = value;
        ////        Database.SetInitializer(Initializer);
        ////    }
        ////}

        #endregion


        #region Implementation from EF6 not included in Core

        public virtual Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        #endregion


        private void SetAuditValues(EntityEntry entry)
        {
            var theEntity = (IAuditedEntity) entry.Entity;
            theEntity.UpdatedOn = DateTime.Now;
            theEntity.UpdatedBy = "xx1234";

            if (entry.State == EntityState.Added)
            {
                theEntity.CreatedOn = DateTime.Now;
                theEntity.CreatedBy = "xx1234";
            }
        }

        private void SetSoftDelete(EntityEntry entry)
        {
            entry.State = EntityState.Modified;
            ((ISoftDelete) entry.Entity).IsDeleted = true;
        }

        #region Model Creation

        public ICreateDbModel ModelCreator { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreator = ModelCreator ?? new DefaultDbModelCreator();
            ModelCreator.Create(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Queries

        public IQueryable<TEntity> EagerLoad<TEntity>(IQueryable<TEntity> query,
            Expression<Func<TEntity, object>> expression) where TEntity : Entity
        {
            // Include will eager load data into the query
            if (query != null && expression != null) query = query.Include(expression);
            return query;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : Entity
        {
            // AsNoTracking returns entities that are not attached to the DbContext
            return new EntitySet<TEntity>(Set<TEntity>().AsNoTracking(), this);
        }

        #endregion

        #region Commands

        public TEntity Get<TEntity>(object firstKeyValue, params object[] otherKeyValues) where TEntity : Entity
        {
            if (firstKeyValue == null) throw new ArgumentNullException(nameof(firstKeyValue));
            var keyValues = new List<object> {firstKeyValue};
            if (otherKeyValues != null) keyValues.AddRange(otherKeyValues);
            return Set<TEntity>().Find(keyValues.ToArray());
        }

        public Task<TEntity> GetAsync<TEntity>(object firstKeyValue, params object[] otherKeyValues)
            where TEntity : Entity
        {
            if (firstKeyValue == null) throw new ArgumentNullException(nameof(firstKeyValue));
            var keyValues = new List<object> {firstKeyValue};
            if (otherKeyValues != null) keyValues.AddRange(otherKeyValues);
            return Set<TEntity>().FindAsync(keyValues.ToArray()).AsTask();
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : Entity
        {
            return new EntitySet<TEntity>(Set<TEntity>(), this);
        }

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (Entry(entity).State == EntityState.Detached) Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : Entity
        {
            var entry = Entry(entity);
            entry.State = EntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (Entry(entity).State != EntityState.Deleted)
                Set<TEntity>().Remove(entity);
        }

        public void Reload<TEntity>(TEntity entity) where TEntity : Entity
        {
            Entry(entity).Reload();
        }

        public Task ReloadAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            return Entry(entity).ReloadAsync();
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (Entry(entity).State == EntityState.Detached)
                Set<TEntity>().Attach(entity);
        }

        public EntityState GetState<TEntity>(TEntity entity) where TEntity : Entity
        {
            return Entry(entity).State;
        }

        #endregion

        #region UnitOfWork

        public void DiscardChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(x => x != null))
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
        }

        public Task DiscardChangesAsync()
        {
            var reloadTasks = new List<Task>();
            foreach (var entry in ChangeTracker.Entries().Where(x => x != null))
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        reloadTasks.Add(entry.ReloadAsync());
                        break;
                }

            return Task.WhenAll(reloadTasks);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(x =>
                x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted))
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete) SetSoftDelete(entry);

                if (entry.Entity is IAuditedEntity) SetAuditValues(entry);
            }

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