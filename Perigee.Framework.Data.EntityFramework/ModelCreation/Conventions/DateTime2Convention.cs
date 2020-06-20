namespace Perigee.EntityFramework.ModelCreation.Conventions
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class DateTime2Convention : IEfDbConvention
    {
        public void SetConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var dateTimeProps = entity.GetProperties()
                    .Where(p =>
                        p.PropertyInfo.PropertyType == typeof(DateTime) ||
                        p.PropertyInfo.PropertyType == typeof(DateTime?)
                    );
                foreach (var prop in dateTimeProps) prop.SetColumnType("datetime2");
            }
        }
    }
}