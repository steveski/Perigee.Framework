namespace Perigee.Framework.Base.Entities
{
    public interface IEntity
    {
        object GetId();

    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }

    }

    /// <summary>
    ///     A single unit of relational data that can be identified by a primary key.
    /// </summary>
    public abstract class Entity<TId> : IEntity<TId>
    {
        public virtual TId Id { get; set; }
        public object GetId() => Id;

    }
}
