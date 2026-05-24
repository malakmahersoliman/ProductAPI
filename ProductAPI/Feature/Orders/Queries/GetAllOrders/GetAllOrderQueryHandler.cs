using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderSummaryDto>>
    {
        private readonly AppDbContext _context;

        public GetAllOrderQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderSummaryDto>>
            Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer!.Name,
                    ItemCount = o.OrderItems.Count,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
