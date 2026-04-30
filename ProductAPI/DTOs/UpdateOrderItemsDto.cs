using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs
{
    public class UpdateOrderDto
    {


        [Required]

        public string Status { get; set; } = "";

    }
}