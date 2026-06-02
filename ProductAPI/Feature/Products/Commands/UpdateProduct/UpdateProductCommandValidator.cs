using FluentValidation;

namespace ProductAPI.Feature.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {

        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Dto.CategoryId)
                .NotEmpty();

            RuleFor(x => x.Dto.Price)
                .GreaterThan(0);

            RuleFor(x => x.Dto.Stock)
                .GreaterThanOrEqualTo(0);
        }

    }
}


