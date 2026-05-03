using FluentValidation;

namespace ProductAPI.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandValidator
    : AbstractValidator<UpdateOrderStatusCommand>
{
    private readonly string[] _allowedStatuses =
    {
        "Pending",
        "Completed",
        "Cancelled"
    };

    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Dto.Status)
            .NotEmpty()
            .Must(status => _allowedStatuses.Contains(status))
            .WithMessage("Status must be Pending, Completed, or Cancelled.");
    }
}