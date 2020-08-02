namespace ExampleRestApi
{
    using System.Security.Claims;
    using System.Security.Principal;
    using Autofac;
    using Divergic.Configuration.Autofac;
    using Example.Domain.Customers.Queries;
    using Example.Services;
    using Microsoft.EntityFrameworkCore;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.EntityFramework;
    using Perigee.Framework.Services;
    using Perigee.Framework.Services.Security;

    public class ExampleRestApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ConfigurationModule<JsonResolver<Config>>>();

            var principal = new ClaimsPrincipal(new GenericIdentity("xx4321")); // TODO: Sort out how to get the actual logged on ClaimsPrincipal for non Blazor application
            builder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();

            builder.Register(c =>
            { // TODO: Do some logging in case required fields on the config aren't specified
                var config = c.Resolve<IDatabaseConfig>();

                var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>();
                optionsBuilder = config.InMemory.Enabled
                    ? optionsBuilder.UseInMemoryDatabase(config.InMemory.Name)
                    : optionsBuilder.UseSqlServer(config.ConnectionString);

                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterModule<EntityFrameworkModule>();

            // Turn on the CQRS pipeline in the framework
            builder.RegisterModule<ServicesModule>();

            builder.RegisterModule<ExampleServicesModule>();


            var autoMapperModule = new AutoMapperModule();

            // If this line isn't done, assemblies are loaded at launch. Using any symbol from the framework ensures they are.
            // https://dotnetcoretutorials.com/2020/07/03/getting-assemblies-is-harder-than-you-think-in-c/
            var ignoreThisCommand = new CustomersBy { FirstName = "blah" };

        }
    }
}