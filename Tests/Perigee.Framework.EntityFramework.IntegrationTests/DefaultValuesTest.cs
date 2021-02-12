using Autofac;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Perigee.Framework.Base.Database;
using Perigee.Framework.Base.Services;
using Perigee.Framework.Base.Transactions;
using Perigee.Framework.EntityFramework.IntegrationTests.Domain;
using Perigee.Framework.Services;
using Perigee.Framework.Services.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Perigee.Framework.EntityFramework.IntegrationTests
{
    public class DefaultValuesTest
    {
        private IContainer Container { get; set; }

        public DefaultValuesTest()
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
        public async Task All_Values_Use_Default()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Test 1 Person",
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;
            createdPerson.Name.Should().Be(createPersonCommand.Name);
            createdPerson.Description.Should().Be(createPersonCommand.Description);
            createdPerson.Alive.Should().Be(createPersonCommand.Alive);
            createdPerson.Dead.Should().Be(createPersonCommand.Dead);

        }

        [Fact]
        public async Task Override_Description()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Test 2 Person",
                Description = "A new Description"
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;
            createdPerson.Name.Should().Be(createPersonCommand.Name);
            createdPerson.Description.Should().Be(createPersonCommand.Description);
            createdPerson.Alive.Should().Be(createPersonCommand.Alive);
            createdPerson.Dead.Should().Be(createPersonCommand.Dead);

        }

        [Fact]
        public async Task Override_True_Default_With_False()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Test 1 Person",
                Alive = false,
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;
            createdPerson.Name.Should().Be(createPersonCommand.Name);
            createdPerson.Description.Should().Be(createPersonCommand.Description);
            createdPerson.Alive.Should().Be(createPersonCommand.Alive);
            createdPerson.Dead.Should().Be(createPersonCommand.Dead);

        }

        [Fact]
        public async Task Override_False_Default_With_True()
        {
            var commandProcessor = Container.Resolve<IProcessCommands>();

            var createPersonCommand = new CreatePersonCommand
            {
                Name = "Test 1 Person",
                Dead=true
            };

            await commandProcessor.Execute(createPersonCommand, CancellationToken.None).ConfigureAwait(false);
            var createdPerson = createPersonCommand.CreatedEntity;
            createdPerson.Name.Should().Be(createPersonCommand.Name);
            createdPerson.Description.Should().Be(createPersonCommand.Description);
            createdPerson.Alive.Should().Be(createPersonCommand.Alive);
            createdPerson.Dead.Should().Be(createPersonCommand.Dead);

        }
    }
}
