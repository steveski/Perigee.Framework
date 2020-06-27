namespace Perigee.Framework.Data.Cqrs.Transactions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;

    public interface IPopulateEntities
    {
        Task Populate(object command, CancellationToken cancellationToken);
    }

    public interface IPopulateEntities<T> : IPopulateEntities where T : IDefineCommand
    {
        Task Populate(T command, CancellationToken cancellationToken);
    }

    public abstract class PopulateEntities<T> : IPopulateEntities<T>, IPopulateEntities where T : IDefineCommand
    {
        protected readonly IWriteEntities Entities;

        protected PopulateEntities(IWriteEntities entities)
        {
            Entities = entities;
        }

        public abstract Task Populate(T command, CancellationToken cancellationToken);

        public Task Populate(object command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}