namespace Perigee.Cqrs.Base.Transactions
{
    using System;
    using System.Threading.Tasks;
    using Database;

    public interface IPopulateEntities
    {
        Task Populate(object command);
    }

    public interface IPopulateEntities<T> : IPopulateEntities where T : IDefineCommand
    {
        Task Populate(T command);
    }

    public abstract class PopulateEntities<T> : IPopulateEntities<T>, IPopulateEntities where T : IDefineCommand
    {
        protected readonly IWriteEntities _entities;

        public PopulateEntities(IWriteEntities entities)
        {
            _entities = entities;
        }

        public abstract Task Populate(T command);

        public Task Populate(object command)
        {
            throw new NotImplementedException();
        }
    }
}