namespace Perigee.Framework.Base.Database
{
    using System.Collections.Generic;
    using Perigee.Framework.Base.Entities;

    public interface IAuditedEntityUpdater
    {
        void UpdateAuditFields(IEnumerable<object> addedEntities, IEnumerable<object> updatedEntities);

    }
}
