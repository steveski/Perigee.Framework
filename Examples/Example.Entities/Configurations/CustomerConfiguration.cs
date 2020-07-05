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




            builder.Property(p => p.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime2");
            builder.Property(p => p.UpdatedOn).HasColumnName("UpdatedOn").HasColumnType("datetime2");

            builder.Property(p => p.CreatedBy).HasColumnName("CreatedBy").HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").HasColumnType("nvarchar").HasMaxLength(50);
            
            builder.Property(p => p.Version).HasColumnName("Version").IsRowVersion();


        }

    }
}