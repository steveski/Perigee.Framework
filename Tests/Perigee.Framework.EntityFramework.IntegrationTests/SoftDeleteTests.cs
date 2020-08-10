using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Perigee.Framework.Base.Database;
using System;
using Xunit;

namespace Perigee.Framework.EntityFramework.IntegrationTests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore.DataEncryption;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.Domain;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;
    using Perigee.Framework.Services;
    using Perigee.Framework.Services.Security;

    public class SoftDeleteTests
    {
        private IContainer Container { get; set; }

        public SoftDeleteTests()
        {
            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<EntityDbContext>()
                    .UseInMemoryDatabase("Nootch");
                    //.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SoftDeleteTest;Trusted_Connection=True;");

                return optionsBuilder.Options;
            });


            var userIdentity = new GenericIdentity(Environment.UserDomainName + "\\" + Environment.UserName, "Anonymous");
            var principal = new ClaimsPrincipal(userIdentity);

            builder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();

            builder.RegisterType<DefaultRecordAuthority>().As<IRecordAuthority>();

            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<EntityFrameworkModule>();

            Container = builder.Build();

        }

        [Fact]
        public async Task QuerySoftDeletedRecordsIgnored()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();
            var queryProcessor = Container.Resolve<IProcessQueries>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Hubert Cumberdale",
                Description = "Friend of Salad Fingers. Has a questionable taste"
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;

            var query = new PersonByQuery { Name = "Hubert Cumberdale" };

            var result = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
            var people = result.ToList();
            people.Should().NotBeNull();
            people.Count().Should().Be(1);

            var deletePersonCommand = new DeletePersonCommand { PersonName = createdPerson.Name };
            await commandProcessor.Execute(deletePersonCommand, CancellationToken.None).ConfigureAwait(false);

            var resultAfterDelete = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
            var peopleAfterDelete = resultAfterDelete.ToList();
            peopleAfterDelete.Should().NotBeNull();
            peopleAfterDelete.Count().Should().Be(0);


        }

        [Fact]
        public async Task QuerySoftDeletedRecordsReturned()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();
            var queryProcessor = Container.Resolve<IProcessQueries>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Jeremy Fisher",
                Description = "Another one of Salad Fingers' friends."
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;

            var query = new PersonByQuery { Name = "Jeremy Fisher" };

            var result = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
            var people = result.ToList();
            people.Should().NotBeNull();
            people.Count().Should().Be(1);

            var deletePersonCommand = new DeletePersonCommand { PersonName = createdPerson.Name };
            await commandProcessor.Execute(deletePersonCommand, CancellationToken.None).ConfigureAwait(false);

            // Include deleted records in results
            query.IncludeDeleted = true;

            var resultAfterDelete = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
            var peopleAfterDelete = resultAfterDelete.ToList();
            peopleAfterDelete.Should().NotBeNull();
            peopleAfterDelete.Count().Should().Be(1);


        }

    }
}
