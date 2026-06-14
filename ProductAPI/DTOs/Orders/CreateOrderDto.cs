using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Orders;

public class CreateOrderDto
{

    public int CustomerId { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public bool PaymentShouldSucceed { get; set; } = true;

    public string? PaymentFailureReason { get; set; }

    public List<CreateOrderItemDto> Items { get; set; } = new();
}
