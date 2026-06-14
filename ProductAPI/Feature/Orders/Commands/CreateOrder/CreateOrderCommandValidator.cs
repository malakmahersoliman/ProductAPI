using FluentValidation;

namespace ProductAPI.Feature.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private static readonly string[] AllowedPaymentMethods =
        {
            "Cash",
            "Card",
            "Wallet",
            "BankTransfer"
        };

        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Dto.CustomerId)
                .GreaterThan(0)
                .WithMessage("Customer is required.");

            RuleFor(x => x.Dto.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .Must(method => AllowedPaymentMethods.Contains(method))
                .WithMessage("Payment method must be Cash, Card, Wallet, or BankTransfer.");

            RuleFor(x => x.Dto.PaymentFailureReason)
                .NotEmpty()
                .When(x => x.Dto.PaymentShouldSucceed == false)
                .WithMessage("Failure reason is required when payment fails.");

            RuleFor(x => x.Dto.Items)
                .NotEmpty()
                .WithMessage("Order must contain at least one item.");

            RuleForEach(x => x.Dto.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.ProductId)
                        .GreaterThan(0)
                        .WithMessage("Product is required.");

                    item.RuleFor(x => x.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero.");
                });
        }
    }
}