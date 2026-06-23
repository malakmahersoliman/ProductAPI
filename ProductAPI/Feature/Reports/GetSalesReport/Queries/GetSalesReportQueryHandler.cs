using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.GetSalesReport;

namespace ProductAPI.Feature.Reports.GetSalesReport.Queries;

public class GetSalesReportQueryHandler
    : IRequestHandler<GetSalesReportQuery, SalesReportDto>
{
    private readonly AppDbContext _context;

    public GetSalesReportQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesReportDto> Handle(
        GetSalesReportQuery request,
        CancellationToken cancellationToken)
    {
        var from = request.From.Date;
        var toExclusive = request.To.Date.AddDays(1);

        var orders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= from && o.OrderDate < toExclusive)
            .Select(o => new SalesReportItemDto
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                CustomerName = o.Customer!.Name,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync(cancellationToken);

        var totalRevenue = orders.Sum(o => o.TotalAmount);
        var totalOrders = orders.Count;

        return new SalesReportDto
        {
            From = request.From,
            To = request.To,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            AverageOrderValue = totalOrders == 0 ? 0 : totalRevenue / totalOrders,
            Orders = orders
        };
    }
}