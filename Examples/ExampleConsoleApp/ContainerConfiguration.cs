﻿namespace ExampleConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Example.Domain.Customers.Queries;
    using Example.Domain.Customers.Views;
    using Example.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Perigee.Framework.Base;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework;
    using Perigee.Framework.Services;
    using Perigee.Framework.Services.Security;

    internal static class ContainerConfiguration
    {
        public static IServiceProvider Configure(ClaimsPrincipal principal)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var serviceCollection = new ServiceCollection();
            // Add basic ASP.NET Core services here such as logging

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(serviceCollection);

            containerBuilder.Register(c => new DbContextOptionsBuilder<EntityDbContext>()
                //.UseInMemoryDatabase("Snoogans")
                //.UseSqlServer(_config.GetConnectionString("DefaultConnection"))
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CqrsExampleDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options
            );

            containerBuilder.RegisterModule<EntityFrameworkModule>();

            // Turn on the CQRS pipeline in the framework
            containerBuilder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();
            containerBuilder.RegisterModule<ServicesModule>();

            containerBuilder.RegisterModule<ExampleServicesModule>();

            // Register the entry point for the application
            containerBuilder.RegisterType<AppProcess>().SingleInstance();
            containerBuilder.RegisterType<AppProcessQueuedCommands>().SingleInstance();
            containerBuilder.RegisterType<AppProcessDelete>().SingleInstance();

            

            var container = containerBuilder.Build();

            // If this line isn't done, assemblies are loaded at launch. Using any symbol from the framework ensures they are.
            var asdasd = container.IsRegistered(typeof(IHandleQuery<CustomersBy, IEnumerable<GetCustomerView>>));

            return new AutofacServiceProvider(container);
        }
    }
}
