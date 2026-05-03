using MediatR;
using ProductAPI.DTOs;

namespace ProductAPI.Feature.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductResponseDto>>
    {
    }
}
