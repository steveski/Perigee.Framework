namespace Perigee.Framework.Base.Database
{
    using System.Collections.Generic;

    public interface IAuditedEntityUpdater
    {
        void UpdateAuditFields(IEnumerable<object> addedEntities, IEnumerable<object> updatedEntities);

    }
}
