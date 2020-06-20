namespace Perigee.Cqrs.Base.Transactions
{
    using Entities;

    public abstract class BaseCreateEntityCommand<TEntity> : BaseEntityCommand where TEntity : Entity
    {
        public TEntity CreatedEntity { get; set; }
    }
}