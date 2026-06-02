using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Products;


namespace ProductAPI.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ProductResponseDto?>
{
    private readonly AppDbContext _context;

    public GetProductByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductResponseDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Price = p.Price,
                Stock = p.Stock,
                IsAvailable = p.IsAvailable
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}