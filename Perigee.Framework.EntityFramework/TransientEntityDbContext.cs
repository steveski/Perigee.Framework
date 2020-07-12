namespace Perigee.Framework.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Services.User;

    public class TransientEntityDbContext : EntityDbContext, ITransientContext
    {
        public TransientEntityDbContext(DbContextOptions<EntityDbContext> options, IRecordAuthority recordAuthority, IAuditedEntityUpdater auditedEntityUpdater) 
            : base(options, recordAuthority, auditedEntityUpdater)
        {
        }
    }
}
