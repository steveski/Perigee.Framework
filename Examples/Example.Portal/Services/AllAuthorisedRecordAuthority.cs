using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Entities;
using System;
using System.Linq.Expressions;

namespace Example.Portal.Services
{
    public class AllAuthorisedRecordAuthority : IRecordAuthority
    {
        public Expression<Func<TEntity, bool>> Clause<TEntity>() where TEntity : IEntity
        {
            return x => true;
        }
    }
}
