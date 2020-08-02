namespace Example.Domain.Customers.Validation
{
    using System.Linq;
    using Example.Domain.Customers.Commands;
    using Example.Entities;
    using FluentValidation;
    using Perigee.Framework.Base.Database;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(command => command.FirstName).NotNull().NotEmpty();
            RuleFor(command => command.LastName).NotNull().NotEmpty();
            RuleFor(command => command.EmailAddress).NotNull().NotEmpty().EmailAddress();


        }

    }
}
