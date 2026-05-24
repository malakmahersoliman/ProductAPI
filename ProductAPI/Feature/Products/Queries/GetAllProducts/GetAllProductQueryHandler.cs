using MediatR;
using ProductAPI.Data;
using ProductAPI.DTOs.Products;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Feature.Products.Queries.GetAllProducts
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetAllProductQueryHandler(AppDbContext context)
        {
            _context = context;
        }   
        public async Task<List<ProductResponseDto>> 
            Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Price = p.Price,
                Stock = p.Stock,
                IsAvailable = p.IsAvailable
            })
            .ToListAsync(cancellationToken);

        }
    }
}
