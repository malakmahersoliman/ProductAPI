using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Queries.GetAllCategories
{
    public class GetAllCategoryQueryHandler
        : IRequestHandler<GetAllCategoriesQuery, List<CategoryResponseDto>>
    {
        private readonly AppDbContext _context;
        public GetAllCategoryQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryResponseDto>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync(cancellationToken);
        }
    }
}

