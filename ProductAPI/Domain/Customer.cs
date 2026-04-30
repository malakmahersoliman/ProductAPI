using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public List<Order> Orders { get; set; } = new();
    }
}
