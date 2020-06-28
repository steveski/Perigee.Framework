namespace Perigee.Framework.EntityFramework
{
    using Autofac;
    using Microsoft.EntityFrameworkCore;
    using ModelCreation;
    using Perigee.Framework.Base;

    /// <summary>
    /// Registers the EntityDbContext and access interfaces IUnitOfWork, IReadEntities and IWriteEntities.
    /// <remarks>This should be registered by instance rather than type.
    /// <example>
    /// var dbContextOptions = new DbContextOptions<EntityDbContext>(...);
    /// builder.RegisterModule(new EntityDbContext(dbContextOptions));
    /// </example></remarks>
    /// </summary>
    public class EntityFrameworkModule : Module
    {
        private readonly DbContextOptions<EntityDbContext> _dbContextOptions;

        public EntityFrameworkModule(DbContextOptions<EntityDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // If the calling code hasn't already registered a custom 
            // ICreateDbModel then register the internal DefaultDbModelCreator
            builder.RegisterType<DefaultDbModelCreator>()
                .IfNotRegistered(typeof(ICreateDbModel))
                .As<ICreateDbModel>();

            // Expecting IUnitOfWork, IReadEntities and IWriteEntities to be registered with this call
            builder.Register(c => new EntityDbContext(_dbContextOptions, c.Resolve<IUserService>())
                {
                    ModelCreator = c.Resolve<ICreateDbModel>()
                })
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

        }
    }
}