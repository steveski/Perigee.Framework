namespace Perigee.EntityFramework.ModelCreation.Conventions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class SoftDeleteConvention : IEfDbConvention
    {
        public void SetConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //if (entity.ClrType is ISoftDelete)
                //{
                var isDeletedProps = entity.GetProperties().Where(p => p.Name == "IsDeleted");
                isDeletedProps.ToList().ForEach(p =>
                {
                    p.IsNullable = false;
                    p.SetColumnType("bit");
                });


                //}
            }
        }
    }
}