using MediatR;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderResponseDto?>
    {
        public int Id { get; set; }
        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
