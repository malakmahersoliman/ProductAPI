namespace ProductAPI.DTOs.GetSalesReport
{
    public class SalesReportItemDto
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
