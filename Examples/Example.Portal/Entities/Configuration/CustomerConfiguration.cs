using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perigee.Framework.EntityFramework;

namespace Example.Portal.Entities.Configuration
{
    public class CustomerConfiguration : BaseEntityTypeConfiguration<Customer>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            builder.Property(p => p.customerName)
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            builder.HasKey("Id");

            builder.ToTable("Customers");
        }
    }
}
