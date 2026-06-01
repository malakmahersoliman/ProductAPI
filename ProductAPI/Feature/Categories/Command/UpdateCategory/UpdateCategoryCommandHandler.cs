using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Categories;
using ProductAPI.Feature.Categories.Commands.UpdateCategory;

namespace ProductAPI.Feature.Categories.Command.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponseDto?>
    {
        private readonly AppDbContext _context;
        public UpdateCategoryCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponseDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                           .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return null;
            }

            var name = request.Dto.Name.Trim();

            var duplicateExists = await _context.Categories
                .AnyAsync(c =>
                    c.Id != request.Id &&
                    c.Name == name,
                    cancellationToken);

            if (duplicateExists)
            {
                throw new InvalidOperationException("Category name already exists.");
            }

            category.Name = name;
            category.Description = request.Dto.Description?.Trim();

            await _context.SaveChangesAsync(cancellationToken);

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
