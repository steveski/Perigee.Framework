namespace Perigee.Framework.Base.Transactions
{
    using Entities;

    public abstract class BaseCreateEntityCommand<TEntity> : BaseEntityCommand where TEntity : IEntity
    {
        public TEntity CreatedEntity { get; set; }
    }
}