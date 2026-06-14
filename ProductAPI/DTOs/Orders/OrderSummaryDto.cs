namespace ProductAPI.DTOs.Orders
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public int ItemCount { get; set; }

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}