namespace Perigee.Framework.Base.Database
{
    using System;
    using System.Linq.Expressions;
    using Perigee.Framework.Base.Entities;

    public interface IRecordAuthority
    {
        Expression<Func<TEntity, bool>> Clause<TEntity>() where TEntity : IEntity;

    }
}
