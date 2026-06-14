using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Common;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public class GetAllOrderQueryHandler
        : IRequestHandler<GetAllOrdersQuery, PagedResultDto<OrderSummaryDto>>
    {
        private readonly AppDbContext _context;

        public GetAllOrderQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<OrderSummaryDto>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            var query = _context.Orders
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(o =>
                    o.Customer!.Name.Contains(filter.Search) ||
                    o.Id.ToString().Contains(filter.Search));
            }

            if (filter.CustomerId.HasValue)
            {
                query = query.Where(o => o.CustomerId == filter.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(o => o.Status == filter.Status);
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
            {
                query = query.Where(o => o.PaymentStatus == filter.PaymentStatus);
            }

            var totalCount = await query.CountAsync(cancellationToken);


            query = query
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.Id);

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    PaymentStatus = o.PaymentStatus,
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer!.Name,
                    ItemCount = o.OrderItems.Count,

                    Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        Id = oi.Id,
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Subtotal = oi.Quantity * oi.UnitPrice
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<OrderSummaryDto>
            {
                Items = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }
    }
}