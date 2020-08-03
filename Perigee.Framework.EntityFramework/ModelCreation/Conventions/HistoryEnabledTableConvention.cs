namespace Perigee.Framework.EntityFramework.ModelCreation.Conventions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Perigee.Framework.Base.Database;

    public class HistoryEnabledTableConvention : IEfDbConvention
    {
        public void SetConvention(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetInterfaces().ToList().Any(t => t.Name == nameof(IHistoryEnabled)))
                {
                    //modelBuilder.Entity<TEntity>(b => b.UseTemporalTable());
                }

            }

        }
    }
}
