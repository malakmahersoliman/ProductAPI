using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs.Products;

public class CreateProductDto
{
    public string Name { get; set; } = "";
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; } = true;
    public string? ImagePath { get; set; }
}
