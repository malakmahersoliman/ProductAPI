namespace ProductAPI.DTOs.Orders;

public class OrderSummaryDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public int ItemCount { get; set; }
}
