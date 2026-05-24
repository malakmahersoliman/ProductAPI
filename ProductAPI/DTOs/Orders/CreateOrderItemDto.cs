using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Orders;

public class CreateOrderItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
