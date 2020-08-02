namespace ExampleRestApi.UnitTests.Customers.Commands
{
    using System;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Example.Domain.Customers.Commands;
    using Example.Entities;
    using FluentAssertions;
    using ModelBuilder;
    using NSubstitute;
    using Perigee.Framework.Base.Database;
    using Perigee.Framework.Base.Services;
    using Xunit;

    public class HandleCreateCustomerCommandTests
    {

        [Fact]
        public async Task HandlePassesCorrectEntityToCreate()
        {
            var testIdentityName = "SomeUserName";

            // Arrange
            var db = Substitute.For<IWriteEntities>();
            var userService = Substitute.For<IUserService>();
            var claimsIdentity = Substitute.For<ClaimsIdentity>();

            userService.ClaimsIdentity.Returns(claimsIdentity);
            claimsIdentity.Name.Returns(testIdentityName);

            var command = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = "Scrackle",
                EmailAddress = "Herbert.Scrackle@home.com"
            };
            
            // Act
            var sut = new HandleCreateCustomerCommand(db, userService);
            await sut.Handle(command, CancellationToken.None);

            // Assert
            db.Received(1).Create(
                // You can just choose to specify that any instance of an object being passed in is fine
                //Arg.Any<Customer>()

                // but when test to make sure this command will pass all the required fields through then this is best
                Arg.Is<Customer>(c =>
                    c.FirstName == command.FirstName &&
                    c.LastName == command.LastName &&
                    c.EmailAddress == command.EmailAddress &&
                    c.ManagedBy == testIdentityName
                ));
            command.CreatedEntity.Should().NotBeNull();

        }

        [Fact]
        public void CreateThrowsExceptionWithNullDb()
        {
            var userService = Substitute.For<IUserService>();

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new HandleCreateCustomerCommand(null, userService);

            action.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public void CreateThrowsExceptionWithNullUserService()
        {
            var db = Substitute.For<IWriteEntities>();

            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new HandleCreateCustomerCommand(db, null);

            action.Should().Throw<ArgumentNullException>();

        }


    }
}
