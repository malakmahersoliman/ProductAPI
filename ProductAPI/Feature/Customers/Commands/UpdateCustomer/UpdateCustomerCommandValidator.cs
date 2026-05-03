using FluentValidation;
using ProductAPI.Features.Customers.Commands.UpdateCustomer;

namespace ProductAPI.Feature.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than 0.");

            RuleFor(c => c.Dto.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Dto.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .MaximumLength(200)
                .WithMessage("Email must be a valid email address and less than 200 characters.");


        }

    }
}
