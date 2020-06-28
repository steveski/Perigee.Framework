namespace Perigee.Framework.EntityFramework
{
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    ///     This is to allow the EF Migrations code to generate the database
    /// </summary>
    public class EntityDbContextFactory : IDesignTimeDbContextFactory<EntityDbContext>
    {
        // TODO: Rework this to have details injected from calling application
        public EntityDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new EntityDbContext(optionsBuilder.Options, null);
        }
    }
}