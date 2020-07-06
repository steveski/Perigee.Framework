namespace Example.Entities.Configurations
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Perigee.Framework.EntityFramework;

    public class CustomerConfiguration : BaseEntityTypeConfiguration<Customer>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.FirstName)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.EmailAddress)
                .HasColumnType("nvarchar(255)")
                .HasMaxLength(255);

            

        }

    }
}