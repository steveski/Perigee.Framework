namespace ExampleConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using Example.Domain.Customers.Queries;
    using Example.Domain.Customers.Views;
    using Example.Mappings;
    using Example.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.DataEncryption;
    using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
    using Microsoft.Extensions.DependencyInjection;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework;
    using Perigee.Framework.Services;
    using Perigee.Framework.Services.Security;

    internal static class ContainerConfiguration
    {
        public static IServiceProvider Configure(ClaimsPrincipal principal, IDatabaseConfig databaseConfig, IAesConfig aesConfig)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var serviceCollection = new ServiceCollection();
            // Add basic ASP.NET Core services here such as logging

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(serviceCollection);

            containerBuilder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>();
                optionsBuilder = databaseConfig.InMemory.Enabled
                    ? optionsBuilder.UseInMemoryDatabase(databaseConfig.InMemory.Name)
                    : optionsBuilder.UseSqlServer(databaseConfig.ConnectionString);

                    return optionsBuilder.Options;
            });

            containerBuilder.RegisterModule<EntityFrameworkModule>();

            // Turn on the CQRS pipeline in the framework
            containerBuilder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();
            containerBuilder.RegisterModule<ServicesModule>();

            containerBuilder.RegisterModule<ExampleServicesModule>();

            // Register the entry point for the application
            containerBuilder.RegisterType<AppProcess>().SingleInstance();
            containerBuilder.RegisterType<AppProcessQueuedCommands>().SingleInstance();
            containerBuilder.RegisterType<AppProcessDelete>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var key = Encoding.ASCII.GetBytes(aesConfig.Key);
                var iv = Encoding.ASCII.GetBytes(aesConfig.Iv);

                return new AesProvider(key, iv);
            }).As<IEncryptionProvider>();

            containerBuilder.Register(c =>
            {
                var scope = c.Resolve<ILifetimeScope>();
                return new MapperConfiguration(mc =>
                    mc.AddProfiles(
                    new List<Profile>
                    {
                        new CommandToEntityProfile(),
                        new EntityToViewProfile()
                    }
                )).CreateMapper();
            }).As<IMapper>().InstancePerLifetimeScope();

            var container = containerBuilder.Build();

            // If this line isn't done, assemblies are loaded at launch. Using any symbol from the framework ensures they are.
            var asdasd = container.IsRegistered(typeof(IHandleQuery<CustomersBy, IEnumerable<GetCustomerWithAddressView>>));

            return new AutofacServiceProvider(container);
        }
    }
}

