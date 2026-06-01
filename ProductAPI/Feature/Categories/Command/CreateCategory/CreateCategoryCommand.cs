using MediatR;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryResponseDto>
    {
        public CategoryRequestDto Dto { get; set; }

        public CreateCategoryCommand(CategoryRequestDto dto)
        {
            Dto = dto;
        }
    }
}