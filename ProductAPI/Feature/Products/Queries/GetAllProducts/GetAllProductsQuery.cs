using MediatR;
using ProductAPI.DTOs.Products;

namespace ProductAPI.Feature.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductResponseDto>>
    {
    }
}
