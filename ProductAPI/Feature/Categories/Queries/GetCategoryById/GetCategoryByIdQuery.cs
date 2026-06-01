using MediatR;
using ProductAPI.DTOs.Categories;

namespace ProductAPI.Feature.Categories.Queries.GetCategoryById
{

    public class GetCategoryByIdQuery : IRequest<CategoryResponseDto>
    {
        public int Id { get;  }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }

    }
}
