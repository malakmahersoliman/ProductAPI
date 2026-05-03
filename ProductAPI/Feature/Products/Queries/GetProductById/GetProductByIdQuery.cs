using MediatR;
using ProductAPI.DTOs;


namespace ProductAPI.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductResponseDto?>
{
    public int Id { get; }

    public GetProductByIdQuery(int id)
    {
        Id = id;
    }
}