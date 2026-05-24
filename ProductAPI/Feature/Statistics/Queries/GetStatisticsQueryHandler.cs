using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Statistics;

namespace ProductAPI.Feature.Statistics.Queries
{
    public class GetStatisticsQueryHandler
        : IRequestHandler<GetStatisticsQuery, StatisticsDto>
    {
        private readonly AppDbContext _context;

        public GetStatisticsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StatisticsDto> Handle(
            GetStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var todayStart = DateTime.UtcNow.Date;
            var tomorrowStart = todayStart.AddDays(1);

            var products = new ProductStatisticsDto
            {
                Total = await _context.Products
                    .CountAsync(cancellationToken),

                Available = await _context.Products
                    .CountAsync(p =>
                        p.IsAvailable &&
                        p.Stock > 0,
                        cancellationToken),

                OutOfStock = await _context.Products
                    .CountAsync(p =>
                        !p.IsAvailable ||
                        p.Stock == 0,
                        cancellationToken),

                LowStock = await _context.Products
                    .CountAsync(p =>
                        p.IsAvailable &&
                        p.Stock > 0 &&
                        p.Stock <= 4,
                        cancellationToken)
            };

            var orders = new OrderStatisticsDto
            {
                Total = await _context.Orders
                    .CountAsync(cancellationToken),

                Pending = await _context.Orders
                    .CountAsync(o => o.Status == "Pending", cancellationToken),

                Completed = await _context.Orders
                    .CountAsync(o => o.Status == "Completed", cancellationToken),

                Cancelled = await _context.Orders
                    .CountAsync(o => o.Status == "Cancelled", cancellationToken),

                TodaySales = await _context.Orders
                    .Where(o =>
                        o.Status == "Completed" &&
                        o.CreatedAt >= todayStart &&
                        o.CreatedAt < tomorrowStart)
                    .SumAsync(o => o.TotalAmount, cancellationToken)
            };

            var customers = new CustomerStatisticsDto
            {
                Total = await _context.Customers
                    .CountAsync(cancellationToken)
            };

            var users = new UserStatisticsDto
            {
                Total = await _context.Users
                    .CountAsync(cancellationToken),

                SuperAdmins = await _context.Users
                    .CountAsync(u => u.Role == "SuperAdmin", cancellationToken),

                RegularUsers = await _context.Users
                    .CountAsync(u => u.Role != "SuperAdmin", cancellationToken)
            };

            return new StatisticsDto
            {
                Products = products,
                Orders = orders,
                Customers = customers,
                Users = users
            };
        }
    }
}