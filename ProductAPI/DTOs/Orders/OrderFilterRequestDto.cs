using ProductAPI.DTOs.Common;

namespace ProductAPI.DTOs.Orders
{
    public class OrderFilterRequestDto : SearchablePaginationRequestionDto
    {
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }

        public int? CustomerId { get; set; }

        public decimal? MinTotalAmount { get; set; }
        public decimal? MaxTotalAmount { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public OrderFilterRequestDto() {
            SortBy = "createdAt"; //back if not right
            SortDirection = "desc";

        }
    }
}
