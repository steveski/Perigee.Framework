using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perigee.Framework.EntityFramework;

namespace Example.Entities.Configurations
{
    public class CustomerEmployerMappingConfiguration : BaseEntityTypeConfiguration<CustomerEmployerMapping>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CustomerEmployerMapping> builder)
        {
            builder.ToTable("CustomerEmployerMapping");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            builder.HasOne(cem => cem.Customer)
                .WithMany(c => c.CustomerEmployerMappings)
                .HasForeignKey(cem => cem.CustomerId);

            builder.HasOne(cem => cem.Employer)
                .WithMany(e => e.CustomerEmployerMappings)
                .HasForeignKey(cem => cem.EmployerId);
        }
    }
}
