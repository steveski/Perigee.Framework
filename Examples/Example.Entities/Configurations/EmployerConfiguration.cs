using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perigee.Framework.EntityFramework;

namespace Example.Entities.Configurations
{
    public class EmployerConfiguration : BaseEntityTypeConfiguration<Employer>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Employer> builder)
        {
            builder.ToTable("Employer");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
