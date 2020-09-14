using Microsoft.EntityFrameworkCore;

namespace Example.Portal.DbMigrations
{
    public class ExamplePortalDbContext : DbContext
    {
        public ExamplePortalDbContext() : base (
                new DbContextOptionsBuilder<ExamplePortalDbContext>()
                    .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Example.Portal;Trusted_Connection=True")
                    .Options)
        {
            // Don't do migrations for InMemory database (ie testing)
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                Database.Migrate();
            }
        }
    }
}
