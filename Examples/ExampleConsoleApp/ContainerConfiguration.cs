namespace ExampleConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Example.Domain.Customers.Queries;
    using Example.Domain.Customers.Views;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Perigee.Framework.Base;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework;
    using Perigee.Framework.Services;

    internal static class ContainerConfiguration
    {
        public static IServiceProvider Configure(ClaimsPrincipal principal)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var serviceCollection = new ServiceCollection();
            // Add basic ASP.NET Core services here such as logging

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(serviceCollection);
            
            var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>()
                .UseInMemoryDatabase("Snoogans");
                //.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
                //.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CqrsExampleDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            var efModule = new EntityFrameworkModule(optionsBuilder.Options);
            
            containerBuilder.RegisterModule(efModule);

            var servicesModule = new ServicesModule(principal);
            containerBuilder.RegisterModule(servicesModule);
            
            containerBuilder.RegisterModule<BaseModule>();

            containerBuilder.RegisterType<AppProcess>().SingleInstance();

            var container = containerBuilder.Build();

            var asdasd = container.IsRegistered(typeof(IHandleQuery<CustomersBy, IEnumerable<GetCustomerView>>));

            return new AutofacServiceProvider(container);
        }
    }
}
