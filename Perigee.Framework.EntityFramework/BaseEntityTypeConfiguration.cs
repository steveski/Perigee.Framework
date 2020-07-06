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
            
            if (typeof(IAuditedEntity).IsAssignableFrom(typeof(TEntity)))
            {
                typeof(EntityBaseConfigurationExtensions).GetMethod(nameof(EntityBaseConfigurationExtensions.ConfigureAudited))
                    ?.MakeGenericMethod(typeof(TEntity))
                    .Invoke(null, new object[] { builder });
            }

            if (typeof(ITimestampEnabled).IsAssignableFrom(typeof(TEntity)))
            {
                typeof(EntityBaseConfigurationExtensions).GetMethod(nameof(EntityBaseConfigurationExtensions.ConfigureTimestamp))
                    ?.MakeGenericMethod(typeof(TEntity))
                    .Invoke(null, new object[] { builder });
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                typeof(EntityBaseConfigurationExtensions).GetMethod(nameof(EntityBaseConfigurationExtensions.ConfigureSoftDelete))
                    ?.MakeGenericMethod(typeof(TEntity))
                    .Invoke(null, new object[] { builder });
            }


        }

        public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    }
    
    public static class EntityBaseConfigurationExtensions
    {
        public static void ConfigureAudited<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IAuditedEntity
        {
            builder.Property(p => p.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime2");
            builder.Property(p => p.UpdatedOn).HasColumnName("UpdatedOn").HasColumnType("datetime2");

            builder.Property(p => p.CreatedBy).HasColumnName("CreatedBy").HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.UpdatedBy).HasColumnName("UpdatedBy").HasColumnType("nvarchar").HasMaxLength(50);

        }

        public static void ConfigureTimestamp<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, ITimestampEnabled
        {
            builder.Property(p => p.Version).HasColumnName("Version").IsRowVersion();

        }
        public static void ConfigureSoftDelete<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, ISoftDelete
        {
            builder.Property(p => p.IsDeleted).HasColumnName("IsDeleted").HasColumnType("bit");

        }

    }

}
