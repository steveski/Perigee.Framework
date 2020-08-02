namespace Example.DbMigrations
{
    using Example.Entities;
    using Example.Entities.Configurations;
    using Microsoft.EntityFrameworkCore;


    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> contextOptions) : base(contextOptions)
        {
            if (Database.ProviderName != "Microsoft.EntityFrameworkCode.InMemory")
            {
                Database.Migrate();
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());

        }
    }

}
