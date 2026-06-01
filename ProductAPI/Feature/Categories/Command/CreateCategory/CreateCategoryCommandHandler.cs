using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Categories;
using ProductAPI.Domain;
using ProductAPI.Feature.Categories.Commands.CreateCategory;




namespace ProductAPI.Feature.Categories.Command.CreateCategory
{

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponseDto>
    {
        private readonly AppDbContext _context;
        public CreateCategoryCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponseDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
           var name = request.Dto.Name.Trim();
            //Trim : Remove whitespace from the beginning and end of a string.
            //name = "  Electronics  " => name = "Electronics" this what happens 
            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower(), cancellationToken);
            //to check if the category with the same name already exists in the database
            //ignoring case sensitivity.
            if (exists)
            {
                throw new InvalidOperationException($"Category with name '{name}' already exists.");
            }               
            var category = new Category
            {
                Name = name,
                Description = request.Dto.Description?.Trim()
            };
            _context.Categories.Add(category);
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
