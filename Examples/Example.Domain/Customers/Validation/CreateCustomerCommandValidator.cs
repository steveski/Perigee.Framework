namespace Example.Domain.Customers.Validation
{
    using Example.Domain.Customers.Commands;
    using FluentValidation;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(command => command.FirstName).NotNull().NotEmpty();
            RuleFor(command => command.LastName).NotNull().NotEmpty();
            RuleFor(command => command.EmailAddress).NotNull().NotEmpty();

        }

    }
}
