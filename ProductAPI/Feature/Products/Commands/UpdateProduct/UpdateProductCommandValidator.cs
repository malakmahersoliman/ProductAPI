using FluentValidation;

namespace ProductAPI.Feature.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {

        public UpdateProductCommandValidator() { 
          RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0.");
            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(200)
                .WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.Dto.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product price must be greater than or equal to 0.");
            RuleFor(x=> x.Dto.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock must be greater than or equal to 0.");

        }
    }
}
