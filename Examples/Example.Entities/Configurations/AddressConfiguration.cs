namespace Example.Entities.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Perigee.Framework.Base.Entities;
    using Perigee.Framework.EntityFramework;

    public class AddressConfiguration : BaseEntityTypeConfiguration<Address>, ITemporalTable
    {
        public override void ConfigureEntity(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.Street).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(p => p.Suburb).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(p => p.State).HasMaxLength(30).IsRequired().HasColumnType("nvarchar(30)");
            builder.Property(p => p.PostalCode).HasMaxLength(10).IsRequired().HasColumnType("nvarchar(10)");
            builder.Property(p => p.Country).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");

            

        }
    }
}
