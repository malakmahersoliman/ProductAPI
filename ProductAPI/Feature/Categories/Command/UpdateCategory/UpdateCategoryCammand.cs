using MediatR;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryResponseDto?>
    {
        public int Id { get; set; }
        public CategoryRequestDto Dto { get; set; }

        public UpdateCategoryCommand(int id, CategoryRequestDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}