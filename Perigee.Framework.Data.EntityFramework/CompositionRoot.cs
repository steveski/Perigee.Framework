namespace Perigee.Framework.Data.EntityFramework
{
    using System.Reflection;
    using Cqrs.Database;
    using Microsoft.EntityFrameworkCore;
    using ModelCreation;
    using SimpleInjector;

    public static class CompositionRoot
    {
        public static void RegisterDatabase(this Container container,
            DbContextOptions<EntityDbContext> dbContextOptions, params Assembly[] assemblies)
        {
            var scopedLifeStyle = container.Options.DefaultScopedLifestyle;

            //container.Register<ICreateDbModel, DefaultDbModelCreator>(scopedLifeStyle); // lifestyle can set here, sometimes you want to change the default lifestyle like singleton exeptionally
            container.RegisterInitializer<EntityDbContext>( //(container.InjectProperties);
                handlerToInitialise => handlerToInitialise.ModelCreator = new DefaultDbModelCreator()
            );

            // Setup DbContext
            var ctxReg = scopedLifeStyle.CreateRegistration(
                () => new EntityDbContext(dbContextOptions),
                container);

            container.AddRegistration<IUnitOfWork>(ctxReg);
            container.AddRegistration<IReadEntities>(ctxReg);
            container.AddRegistration<IWriteEntities>(ctxReg);
        }
    }
}