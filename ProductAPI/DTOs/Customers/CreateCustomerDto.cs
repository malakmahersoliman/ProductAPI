using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Customers;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = "";
}
