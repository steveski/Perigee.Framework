namespace Perigee.Framework.Data.EntityFramework.ModelCreation.Conventions
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    // Disabling the convention by commenting the inheritence.
    // Been getting a strange issue in relation to a property in one case being null
    // datetime2 is default for modern SqlServer anyway so this is not a critical step
    public class DateTime2Convention //: IEfDbConvention
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