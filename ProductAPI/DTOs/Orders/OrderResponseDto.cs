namespace ProductAPI.DTOs.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";

    public string PaymentStatus { get; set; } = string.Empty;

    public string? PaymentMethod { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
    public string CreatedByEmail { get; set; } = string.Empty;
    public int CreatedById { get;  set; }
}
