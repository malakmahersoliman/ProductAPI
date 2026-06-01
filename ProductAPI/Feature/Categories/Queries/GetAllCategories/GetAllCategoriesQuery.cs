using MediatR;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryResponseDto>>
    {
    }
}
