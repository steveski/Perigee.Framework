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
    using Perigee.Framework.Base.Entities;
    using Perigee.Framework.Base.Services;
    using Perigee.Framework.Base.Transactions;
    using Perigee.Framework.EntityFramework.IntegrationTests.Domain;
    using Perigee.Framework.EntityFramework.IntegrationTests.EntitiesForTesting;
    using Perigee.Framework.EntityFramework.ModelCreation;
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

            //builder.Register(c =>
            //    {
            //        IEncryptionProvider encryptionProvider = null;
            //        if (c.IsRegistered<IEncryptionProvider>())
            //            encryptionProvider = c.Resolve<IEncryptionProvider>();

            //        return new EntityDbContext(
            //            c.Resolve<DbContextOptions<EntityDbContext>>(),
            //            c.Resolve<IRecordAuthority>(),
            //            c.Resolve<IAuditedEntityUpdater>(),
            //            encryptionProvider)
            //        {
            //            ModelCreator = c.Resolve<ICreateDbModel>()
            //        };
            //    })
            //    .AsSelf()
            //    .InstancePerDependency();

            var userIdentity = new GenericIdentity(Environment.UserDomainName + "\\" + Environment.UserName, "Anonymous");
            var principal = new ClaimsPrincipal(userIdentity);

            builder.Register(c => new PrincipalProvider(principal)).As<IPrincipalProvider>();

            builder.RegisterType<DefaultRecordAuthority>().As<IRecordAuthority>();

            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<EntityFrameworkModule>();

            Container = builder.Build();

        }

        private void ClearDatabase()
        {
            // Need the actual DbContext subclass directly in order to call to .Database property
            var db = Container.Resolve<EntityDbContext>();
             db.Database.EnsureDeleted();

        }


        ////////////[Fact]
        ////////////public async Task QuerySoftDeletedRecordsIgnored()
        ////////////{
        ////////////    var commandProcessor = Container.Resolve<IProcessCommands>();
        ////////////    var queryProcessor = Container.Resolve<IProcessQueries>();

        ////////////    var createPersonCommand = new CreatePersonCommand
        ////////////    {
        ////////////        Name = "Hubert Cumberdale",
        ////////////        Description = "Friend of Salad Fingers. Has a questionable taste"
        ////////////    };

        ////////////    await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
        ////////////    var createdPerson = createPersonCommand.CreatedEntity;

        ////////////    var query = new PersonByQuery { Name = "Hubert Cumberdale" };

        ////////////    var result = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
        ////////////    var people = result.ToList();
        ////////////    people.Should().NotBeNull();
        ////////////    people.Count().Should().Be(1);

        ////////////    var deletePersonCommand = new DeletePersonCommand { PersonName = createdPerson.Name };
        ////////////    await commandProcessor.Execute(deletePersonCommand, CancellationToken.None).ConfigureAwait(false);

        ////////////    var resultAfterDelete = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
        ////////////    var peopleAfterDelete = resultAfterDelete.ToList();
        ////////////    peopleAfterDelete.Should().NotBeNull();
        ////////////    peopleAfterDelete.Count().Should().Be(0);

        ////////////    ClearDatabase();

        ////////////}

        ////////////[Fact]
        ////////////public async Task QuerySoftDeletedRecordsReturned()
        ////////////{
        ////////////    var commandProcessor = Container.Resolve<IProcessCommands>();
        ////////////    var queryProcessor = Container.Resolve<IProcessQueries>();

        ////////////    var createJeremyCommand = new CreatePersonCommand
        ////////////    {
        ////////////        Name = "Jeremy Fisher",
        ////////////        Description = "Another one of Salad Fingers' friends."
        ////////////    };

        ////////////    var createMarjoryCommand = new CreatePersonCommand
        ////////////    {
        ////////////        Name = "Marjory Stewart Baxter",
        ////////////        Description = "Another one of Salad Fingers' friends."
        ////////////    };

        ////////////    await commandProcessor.Execute(createJeremyCommand, CancellationToken.None).ConfigureAwait(false);
        ////////////    await commandProcessor.Execute(createMarjoryCommand, CancellationToken.None).ConfigureAwait(false);

        ////////////    var createdJeremyPerson = createJeremyCommand.CreatedEntity;
        ////////////    var createdMarjoryPerson = createMarjoryCommand.CreatedEntity;

        ////////////    var deletePersonCommand = new DeletePersonCommand { PersonName = createJeremyCommand.Name };
        ////////////    await commandProcessor.Execute(deletePersonCommand, CancellationToken.None).ConfigureAwait(false);

        ////////////    // Standard query excluding deleted
        ////////////    var query = new PersonByQuery();

        ////////////    var result = await queryProcessor.Execute(query, CancellationToken.None).ConfigureAwait(false);
        ////////////    var people = result.ToList();
        ////////////    people.Should().NotBeNull();
        ////////////    people.Count().Should().Be(1);

        ////////////    // Include deleted records in results
        ////////////    var queryAllWithDeleted = new PersonByQuery { IncludeDeleted = true };

        ////////////    var resultAfterDelete = await queryProcessor.Execute(queryAllWithDeleted, CancellationToken.None).ConfigureAwait(false);
        ////////////    var peopleAfterDelete = resultAfterDelete.ToList();
        ////////////    peopleAfterDelete.Should().NotBeNull();
        ////////////    peopleAfterDelete.Count().Should().Be(2);

        ////////////    ClearDatabase();

        ////////////}

    }
}
