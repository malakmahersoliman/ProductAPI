using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs
{
    public class CreateCustomerDto
    {

        [Required]
        public string Name { get; set; } = "";

        [Required]

        public string Email { get; set; } = "";

    }
}
