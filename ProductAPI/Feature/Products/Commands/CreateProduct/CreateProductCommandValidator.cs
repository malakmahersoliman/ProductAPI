using FluentValidation;

namespace ProductAPI.Feature.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()

        {
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
