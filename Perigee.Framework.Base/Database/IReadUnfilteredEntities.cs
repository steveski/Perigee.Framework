namespace Perigee.Framework.Base.Database
{
    using System.Linq;
    using Perigee.Framework.Base.Entities;

    public interface IReadUnfilteredEntities
    {
        IQueryable<TEntity> QueryUnfiltered<TEntity>() where TEntity : class, IEntity;
    }
}