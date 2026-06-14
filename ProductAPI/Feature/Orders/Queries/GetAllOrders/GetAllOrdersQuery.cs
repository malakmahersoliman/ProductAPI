using MediatR;
using ProductAPI.DTOs.Common;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public record GetAllOrdersQuery(OrderFilterRequestDto Filter)
        : IRequest<PagedResultDto<OrderSummaryDto>>;
}
