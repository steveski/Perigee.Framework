namespace Example.DbMigrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetSection("Database").GetValue<string>("ConnectionString");

            var dbContextOptions = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            
            // ReSharper disable once ObjectCreationAsStatement
            new ExampleDbContext(dbContextOptions);


        }

    }
}
