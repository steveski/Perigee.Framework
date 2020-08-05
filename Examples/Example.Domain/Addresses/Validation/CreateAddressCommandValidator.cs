namespace Example.Domain.Addresses.Validation
{
    using Example.Domain.Addresses.Commands;
    using FluentValidation;

    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(command => command.Street).NotNull().NotEmpty();
            RuleFor(command => command.Suburb).NotNull().NotEmpty();
            RuleFor(command => command.PostalCode).NotNull().NotEmpty();
            RuleFor(command => command.State).NotNull().NotEmpty();
            RuleFor(command => command.Country).NotNull().NotEmpty();
        }
    }
}
