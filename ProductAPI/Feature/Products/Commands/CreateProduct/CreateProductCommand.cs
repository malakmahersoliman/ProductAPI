using MediatR;
using ProductAPI.DTOs.Products;

namespace ProductAPI.Feature.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ProductResponseDto>
    {
        public CreateProductDto Dto {   get; } // dto property to hold the data for creating a product

        public CreateProductCommand(CreateProductDto dto)
        {
            Dto = dto; // constructor to initialize the dto property
        }
    }
}
