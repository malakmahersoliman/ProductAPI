using ProductAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Orders;

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}
