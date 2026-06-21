using System.Data;

namespace ProductAPI.DTOs.GetSalesReport
{
    public class SalesReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal AverageOrderValue { get; set; }

        public List<SalesReportItemDto> Orders { get; set; } =[];
    }
}
