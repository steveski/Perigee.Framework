using Example.Domain.Customers.Commands;
using Example.Domain.Customers.Queries;
using FluentAssertions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExampleConsoleApp.UnitTests
{
    public class UpdateCustomer : TestBase
    {
        [Fact]
        public async Task UpdateCustomerSingleUpdate()
        {
            var custQuery = new CustomersBy();
            var custQueryHandler = new HandleCustomerByQuery(_readDb, _mapper);
            var allCustomers = await custQueryHandler.Handle(custQuery, CancellationToken.None);
            var firstCustomer = allCustomers.FirstOrDefault();
            var custUpdateCommand = new UpdateCustomerCommand
            {
                Id = firstCustomer.Id,
                FirstName = "First name updated",
                LastName = "Last name updated",
                EmailAddress = "Email address updated"
            };
            var sut = new HandleUpdateCustomerCommand(_writeDb);
            await sut.Handle(custUpdateCommand, CancellationToken.None);

            // Get customers and ensure updated
            allCustomers = await custQueryHandler.Handle(custQuery, CancellationToken.None);
            allCustomers.Should().HaveCount(1);
            var updatedCustomer = allCustomers.First();
            updatedCustomer.FirstName.Should().NotBe(firstCustomer.FirstName);
            updatedCustomer.FirstName.Should().Be(custUpdateCommand.FirstName);
        }

        [Fact]
        public async Task UpdateCustomerMultipleUpdates()
        {
            var custQuery = new CustomersBy();
            var custQueryHandler = new HandleCustomerByQuery(_readDb, _mapper);
            var allCustomers = await custQueryHandler.Handle(custQuery, CancellationToken.None);
            var firstCustomer = allCustomers.FirstOrDefault();
            var custUpdateCommand = new UpdateCustomerCommand
            {
                Id = firstCustomer.Id,
                FirstName = "First name updated",
                LastName = "Last name updated",
                EmailAddress = "Email address updated"
            };
            var sut = new HandleUpdateCustomerCommand(_writeDb);
            await sut.Handle(custUpdateCommand, CancellationToken.None);

            // Get customers and ensure updated
            allCustomers = await custQueryHandler.Handle(custQuery, CancellationToken.None);
            allCustomers.Should().HaveCount(1);
            var updatedCustomer = allCustomers.First();
            updatedCustomer.FirstName.Should().NotBe(firstCustomer.FirstName);
            updatedCustomer.FirstName.Should().Be(custUpdateCommand.FirstName);

            var custUpdateCommand2 = new UpdateCustomerCommand
            {
                Id = firstCustomer.Id,
                FirstName = "First name updated again",
                LastName = "Last name updated again",
                EmailAddress = "Email address updated again"
            };
            await sut.Handle(custUpdateCommand2, CancellationToken.None);

            // Get customers and ensure updated
            allCustomers = await custQueryHandler.Handle(custQuery, CancellationToken.None);
            allCustomers.Should().HaveCount(1);
            var updatedCustomer2 = allCustomers.First();
            updatedCustomer2.FirstName.Should().NotBe(firstCustomer.FirstName);
            updatedCustomer2.FirstName.Should().NotBe(custUpdateCommand.FirstName);
            updatedCustomer2.FirstName.Should().Be(custUpdateCommand2.FirstName);
        }
    }
}
