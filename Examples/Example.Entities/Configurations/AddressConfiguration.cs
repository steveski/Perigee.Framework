namespace Example.Entities.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.Street).HasMaxLength(100).IsRequired().HasColumnType("nvarchar");
            builder.Property(p => p.Suburb).HasMaxLength(100).IsRequired().HasColumnType("nvarchar");
            builder.Property(p => p.State).HasMaxLength(30).IsRequired().HasColumnType("nvarchar");
            builder.Property(p => p.PostalCode).HasMaxLength(10).IsRequired().HasColumnType("nvarchar");
            builder.Property(p => p.Country).HasMaxLength(100).IsRequired().HasColumnType("nvarchar");

            

        }
    }
}
