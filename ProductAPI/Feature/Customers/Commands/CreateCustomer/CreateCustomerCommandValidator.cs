using FluentValidation;

namespace ProductAPI.Feature.Customers.Commands.CreateCustomer
{

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Dto.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(200);
        }
    }
}
