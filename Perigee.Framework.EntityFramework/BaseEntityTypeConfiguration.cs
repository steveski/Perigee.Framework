namespace Perigee.Framework.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Perigee.Framework.Base.Entities;

    public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureEntity(builder);

            //var audited = builder as EntityTypeBuilder<IAuditedEntity>;
            //audited?.ConfigureAudited();
            
            //var timestamped = builder as EntityTypeBuilder<ITimestampEnabled>;
            //timestamped?.ConfigureTimestamp();

            //var softDelete = builder as EntityTypeBuilder<ISoftDelete>;
            //softDelete?.ConfigureSoftDelete();

        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);


    }



    public static class EntityBaseConfiguration
    {
        public static void ConfigureAudited(this EntityTypeBuilder<IAuditedEntity> builder)
        {
            builder.Property(p => p.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime2");
            builder.Property(p => p.UpdatedOn).HasColumnName("UpdatedOn").HasColumnType("datetime2");

            builder.Property(p => p.CreatedBy).HasColumnName("CreatedBy").HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.UpdatedOn).HasColumnName("UpdatedBy").HasColumnType("nvarchar").HasMaxLength(50);

        }

        public static void ConfigureTimestamp(this EntityTypeBuilder<ITimestampEnabled> builder)
        {
            builder.Property(p => p.Version).HasColumnName("Version").IsRowVersion();

        }
        public static void ConfigureSoftDelete(this EntityTypeBuilder<ISoftDelete> builder)
        {
            builder.Property(p => p.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");

        }

    }

}
