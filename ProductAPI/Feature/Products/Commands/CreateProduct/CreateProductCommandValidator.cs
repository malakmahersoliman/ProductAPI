using FluentValidation;

namespace ProductAPI.Feature.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator() { 
        
                RuleFor(x => x.Dto.Name)
                    .NotEmpty()
                    .WithMessage("Product name is required.")
                    .MaximumLength(200)
                    .WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Dto.Category)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Dto.Price)
                    .GreaterThan(0)
                    .WithMessage("Price must be greater than zero.");


        }
    }

}
