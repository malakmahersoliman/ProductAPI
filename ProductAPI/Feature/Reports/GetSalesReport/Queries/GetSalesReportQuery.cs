using MediatR;
using ProductAPI.DTOs.GetSalesReport;

namespace ProductAPI.Feature.Reports.GetSalesReport.Queries
{
    public record GetSalesReportQuery(DateTime From, DateTime To) : IRequest<SalesReportDto>;

}
