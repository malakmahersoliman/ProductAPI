using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.Common;
using ProductAPI.DTOs.Orders;
using ProductAPI.services;



namespace ProductAPI.Feature.Orders.Queries.GetAllOrders
{
    public class GetAllOrderQueryHandler
        : IRequestHandler<GetAllOrdersQuery, PagedResultDto<OrderSummaryDto>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetAllOrderQueryHandler(
            AppDbContext context,
            ICurrentUserService currentUser
            )
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResultDto<OrderSummaryDto>> Handle(
            GetAllOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            var query = _context.Orders
                .AsNoTracking()
                .AsQueryable();

            if (!_currentUser.IsSuperAdmin)
            {
                query = query.Where(o => o.CreatedById == _currentUser.UserId);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(o => o.Id.ToString().Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                var customerName = filter.CustomerName.Trim().ToLower();
                query = query.Where(o => o.Customer!.Name.ToLower() == customerName);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                if (Enum.TryParse<OrderStatus>(filter.Status, true, out var status))
                {
                    query = query.Where(o => o.Status == status);
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
            {
                query = query.Where(o => o.PaymentStatus.ToString() == filter.PaymentStatus);
            }

            var totalCount = await query.CountAsync(cancellationToken);


            query = query
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.Id);

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    PaymentStatus = o.PaymentStatus.ToString(),
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
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
    }
}