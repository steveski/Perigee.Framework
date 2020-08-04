namespace Perigee.Framework.EntityFramework.ModelCreation.Conventions
{
    using System.Linq;
    using EntityFrameworkCore.TemporalTables.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Perigee.Framework.Base.Database;

    public class HistoryEnabledTableConvention : IEfDbConvention
    {
        public void SetConvention(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IHistoryEnabled).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType, b => b.UseTemporalTable());

                }

            }

        }
    }
}
