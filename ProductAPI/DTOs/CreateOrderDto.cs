using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Orders;

public class CreateOrderDto
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}