using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.Products;

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
                var categoryExists = await _context.Categories
                     .AnyAsync(c => c.Id == request.Dto.CategoryId, cancellationToken);

                if (!categoryExists)
                {
                    throw new InvalidOperationException("Category does not exist.");
                }
            // 1. Fetch the category name directly during the validation step
            var categoryName = await _context.Categories
                 .Where(c => c.Id == dto.CategoryId)
                 .Select(c => c.Name)
                 .FirstOrDefaultAsync(cancellationToken);

            if (categoryName == null)
            {
                throw new InvalidOperationException("Category does not exist.");
            }
            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Stock = dto.Stock,
                IsAvailable = dto.IsAvailable
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            //await _context.Entry(product).Reference(p => p.Category).LoadAsync(cancellationToken); add the !


            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                CategoryName = categoryName,
                Price = product.Price,
                Stock = product.Stock,
                IsAvailable = product.IsAvailable
            };
        }
    }
}
