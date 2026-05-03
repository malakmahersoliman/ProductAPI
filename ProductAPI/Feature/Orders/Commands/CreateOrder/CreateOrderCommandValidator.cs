using FluentValidation;

namespace ProductAPI.Feature.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Dto.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.Dto.Items)
                .NotEmpty();

            RuleForEach(x => x.Dto.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId)
                        .GreaterThan(0);

                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0);
                });
        }
    }
}
