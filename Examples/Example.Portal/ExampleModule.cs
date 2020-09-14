using Autofac;
using Divergic.Configuration.Autofac;
using Example.Portal.Domain.Customer.Queries;
using Example.Portal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Perigee.Framework.Base.Services;
using Perigee.Framework.EntityFramework;
using Perigee.Framework.Services;
using Perigee.Framework.Services.Security;
using System;
using System.Text;

namespace Example.Portal
{
    public class ExampleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Write a config class to use this
            var resolver = new JsonResolver<Config>(
                /*
                 * Program.secretAppsettingsFile
                 * */
                 );
            var module = new ConfigurationModule(resolver);
            builder.RegisterModule(module);


            /*
            var principal = new ClaimsPrincipal(new GenericIdentity("xx4321")); // TODO: Sort out how to get the actual logged on ClaimsPrincipal for non Blazor application
            builder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();
*/


            //            use the below, but look at lifetime - per request


            builder.Register(c =>
            {
                // This needs updating - to do the right thing when going through the API

                // Need custom Principalprovider????

                //Can it come from the Bearer Token?  Do we have one?


                var httpContextAccessor = c.Resolve<IHttpContextAccessor>();
                var principal = httpContextAccessor.HttpContext.User;
                return new PrincipalProvider(principal);
            }).As<IPrincipalProvider>();


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

            builder.RegisterModule<ExamplePortalServicesModule>();

            /*
            builder.Register(c =>
            {
                var keyInfo = c.Resolve<IAesConfig>();
                var key = Encoding.ASCII.GetBytes(keyInfo.Key);
                var iv = Encoding.ASCII.GetBytes(keyInfo.Iv);

                // Check in UserSecrets for override data
                var systemConfig = c.Resolve<Microsoft.Extensions.Configuration.IConfiguration>();
                if (!string.IsNullOrEmpty(systemConfig["AesKey"]))
                    key = Encoding.ASCII.GetBytes(systemConfig["AesKey"]);
                if (!string.IsNullOrEmpty(systemConfig["AESIv"]))
                    iv = Encoding.ASCII.GetBytes(systemConfig["AESIv"]);

                return new AesProvider(key, iv);
            }).As<IEncryptionProvider>();
*/

            // builder.RegisterType<AllAuthorisedRecordAuthority>().As<IRecordAuthority>().InstancePerLifetimeScope();

            // If this line isn't done, assemblies aren't loaded at launch. Using any symbol from the framework ensures they are.
            var ignoreThisCommand = new CustomerQuery();
        }
    }
}
