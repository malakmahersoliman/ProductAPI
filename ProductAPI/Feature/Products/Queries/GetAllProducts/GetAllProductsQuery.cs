using MediatR;
using ProductAPI.DTOs.Common;
using ProductAPI.DTOs.Products;

namespace ProductAPI.Feature.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<PagedResultDto<ProductResponseDto>>
    {
        public ProductFilterRequestDto Filter { get; set; }
        public GetAllProductsQuery(ProductFilterRequestDto filter)
        {
            Filter = filter;
        }
    }
}
