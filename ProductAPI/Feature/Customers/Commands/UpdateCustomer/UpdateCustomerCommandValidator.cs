using FluentValidation;
using ProductAPI.Features.Customers.Commands.UpdateCustomer;

namespace ProductAPI.Feature.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

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
