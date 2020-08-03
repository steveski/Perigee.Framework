namespace Example.Entities.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Perigee.Framework.EntityFramework;

    public class EmploymentConfiguration : BaseEntityTypeConfiguration<Employment>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Employment> builder)
        {
            builder.Property(p => p.Position).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");
            builder.Property(p => p.Responsibilities).HasMaxLength(300).IsRequired().HasColumnType("nvarchar(300)");
            builder.Property(p => p.StartDate).HasColumnType("datetime2");
            builder.Property(p => p.EndDate).HasColumnType("datetime2").HasDefaultValue(DateTime.MaxValue);
            builder.Property(p => p.ActingRole).HasColumnType("bit").HasDefaultValue(0);

        }

    }
}
