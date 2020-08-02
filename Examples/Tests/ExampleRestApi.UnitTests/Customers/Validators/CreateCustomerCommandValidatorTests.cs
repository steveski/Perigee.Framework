namespace ExampleRestApi.UnitTests.Customers.Validators
{
    using Example.Domain.Customers.Commands;
    using Example.Domain.Customers.Validation;
    using FluentAssertions;
    using ModelBuilder;
    using Xunit;

    public class CreateCustomerCommandValidatorTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("Herbert", true)]
        public void FailsIfFirstNameNotProvided(string firstName, bool expectedValid)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = firstName,
                LastName = "Scrackle",
                EmailAddress = "herbert.scrackle@home.com"
            };

            var sut = new CreateCustomerCommandValidator();

            var result = sut.Validate(command);
            result.IsValid.Should().Be(expectedValid);

        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("Scrackle", true)]
        public void FailsIfLastNameNotProvided(string lastName, bool expectedValid)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = lastName,
                EmailAddress = "herbert.scrackle@home.com"
            };

            var sut = new CreateCustomerCommandValidator();

            var result = sut.Validate(command);
            result.IsValid.Should().Be(expectedValid);

        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("herbert", false)]
        [InlineData("@herb", false)]
        [InlineData("herbert.scrackle@home.com", true)]
        public void FailsIfEmailInvalid(string email, bool expectedValid)
        {
            var command = new CreateCustomerCommand
            {
                FirstName = "Herbert",
                LastName = "Scrackle",
                EmailAddress = email
            };

            var sut = new CreateCustomerCommandValidator();

            var result = sut.Validate(command);
            result.IsValid.Should().Be(expectedValid);

        }


    }
}
