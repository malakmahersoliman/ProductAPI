using MediatR;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQuery :IRequest<List<OrderResponseDto>>
    {
    }
}
