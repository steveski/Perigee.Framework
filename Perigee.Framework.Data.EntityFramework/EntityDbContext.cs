﻿namespace Perigee.Framework.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Cqrs;
    using Cqrs.Database;
    using Cqrs.Entities;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using ModelCreation;

    public class EntityDbContext : DbContext, IWriteEntities
    {
        private readonly IUserService _userService;

        #region Construction & Initialization

        public EntityDbContext(DbContextOptions<EntityDbContext> options, IUserService userService) : base(options)
        {
            Ensure.Any.IsNotNull(userService, nameof(userService));

            _userService = userService;

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


        private void SetAuditValues(EntityEntry entry)
        {
            var theEntity = (IAuditedEntity) entry.Entity;
            theEntity.UpdatedOn = _userService.CurrentDateTime;
            theEntity.UpdatedBy = _userService.Principal.Identity.Name;

            if (entry.State == EntityState.Added)
            {
                theEntity.CreatedOn = _userService.CurrentDateTime;
                theEntity.CreatedBy = _userService.Principal.Identity.Name;
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
            ModelCreator ??= new DefaultDbModelCreator();
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

        public Task<TEntity> GetAsync<TEntity>(object firstKeyValue, CancellationToken cancellationToken, params object[] otherKeyValues)
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
            if (entry.State != EntityState.Added)
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

        public Task ReloadAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : Entity
        {
            return Entry(entity).ReloadAsync(cancellationToken);
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

        public Task DiscardChangesAsync(CancellationToken cancellationToken)
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
                        reloadTasks.Add(entry.ReloadAsync(cancellationToken));
                        break;
                }

            return Task.WhenAll(reloadTasks);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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