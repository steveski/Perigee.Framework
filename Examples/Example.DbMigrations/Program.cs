namespace Example.DbMigrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            
            // ReSharper disable once ObjectCreationAsStatement
            new ExampleDbContext();


        }

    }
}
