using MediatR;
using ProductAPI.DTOs.GetSalesReport;

namespace ProductAPI.Feature.Reports.Queries.GetSalesReport
{
    public record GetSalesReportQuery(DateTime From, DateTime To) : IRequest<SalesReportDto>;

}
