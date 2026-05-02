using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Domain
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
