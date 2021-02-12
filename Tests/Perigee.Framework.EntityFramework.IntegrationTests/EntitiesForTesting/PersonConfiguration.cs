namespace Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasColumnType("nvarchar(100)").HasMaxLength(100).HasColumnName("Name").IsRequired();
            builder.Property(p => p.Age).HasColumnType("int").HasColumnName("Age");
            builder.Property(p => p.Description).HasColumnType("nvarchar(200)").HasMaxLength(200).HasColumnName("Description").IsRequired();
            builder.Property(p => p.Dead).HasColumnType("bit").HasDefaultValue(0);
            builder.Property(p => p.Alive).HasColumnType("bit").HasDefaultValue(1);

        }
    }
}
