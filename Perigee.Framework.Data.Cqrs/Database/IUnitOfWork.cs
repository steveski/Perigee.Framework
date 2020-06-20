﻿namespace Perigee.Cqrs.Base.Database
{
    using System;
    using System.Threading.Tasks;
    using Helpers.Shared;

    /// <summary>
    ///     Synchronizes data state changes with an underlying data store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Commit all current data changes to the underlying data store.
        /// </summary>
        /// <returns>
        ///     The number of data units whose values were modified after saving
        ///     changes.
        /// </returns>
        // ReSharper disable UnusedMethodReturnValue.Global
        int SaveChanges();
        // ReSharper restore UnusedMethodReturnValue.Global

        /// <summary>
        ///     Asynchronously commit all current data changes to the underlying data store.
        /// </summary>
        /// <returns>
        ///     A task result that contains the number of data units whose values were modified after saving
        ///     changes.
        /// </returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        ///     Asynchronously revert all current data changes to the last known state of the underlying data store.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [UsedImplicitly]
        Task DiscardChangesAsync();

        /// <summary>
        ///     Revert all current data changes to the last known state of the underlying data store.
        /// </summary>
        [UsedImplicitly]
        void DiscardChanges();
    }
}