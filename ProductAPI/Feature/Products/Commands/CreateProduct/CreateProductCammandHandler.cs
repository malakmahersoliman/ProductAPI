using MediatR;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs;

namespace ProductAPI.Feature.Products.Commands.CreateProduct
{
    public class CreateProductCammandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {

        private readonly AppDbContext _context;

        public CreateProductCammandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProductResponseDto>
            Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var product = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock,
                IsAvailable = dto.IsAvailable
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
                IsAvailable = product.IsAvailable
            };
        }
    }
}
