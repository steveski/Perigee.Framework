namespace Perigee.EntityFramework.ModelCreation.Conventions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AuditFieldsConvention : IEfDbConvention
    {
        public void SetConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //if (entity.ClrType is IAuditedEntity)
                //{
                var auditDateProps =
                    entity.GetProperties().Where(p => new[] {"CreatedOn", "UpdatedOn"}.Contains(p.Name));
                auditDateProps.ToList().ForEach(p =>
                {
                    p.IsNullable = false;
                    p.SetColumnType("datetime2");
                });

                var auditUserProps =
                    entity.GetProperties().Where(p => new[] {"CreatedBy", "UpdatedBy"}.Contains(p.Name));
                auditUserProps.ToList().ForEach(p =>
                {
                    p.IsNullable = false;
                    p.SetColumnType("nvarchar(100)");
                    p.SetIsUnicode(true);
                });
                //}
            }
        }
    }
}