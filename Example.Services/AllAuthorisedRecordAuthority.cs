namespace Example.Services
{
    using System;
    using System.Linq.Expressions;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Entities;

    public class AllAuthorisedRecordAuthority : IRecordAuthority
    {
        public Expression<Func<TEntity, bool>> Clause<TEntity>() where TEntity : IEntity
        {
            return x => true;
        }
    }
}
