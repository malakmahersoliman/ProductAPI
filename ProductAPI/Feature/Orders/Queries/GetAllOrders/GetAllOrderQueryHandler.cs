using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponseDto>>
    {
        private readonly AppDbContext _context;
        public GetAllOrderQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderResponseDto>> 
            Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToListAsync(cancellationToken);

        }
    }
}
