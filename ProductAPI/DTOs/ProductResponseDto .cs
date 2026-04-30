using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.DTOs
{
    public class ProductResponseDto
    {
       
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsAvailable { get; set; } 
    }
}
