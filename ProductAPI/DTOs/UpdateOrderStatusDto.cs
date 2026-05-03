using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Orders;

public class UpdateOrderStatusDto
{
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "";
}