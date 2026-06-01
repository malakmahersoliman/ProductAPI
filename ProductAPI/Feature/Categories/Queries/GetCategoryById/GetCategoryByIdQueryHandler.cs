using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponseDto?>
    {
        private readonly AppDbContext _context;
        public GetCategoryByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponseDto?> Handle(
            GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Categories
                 .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
