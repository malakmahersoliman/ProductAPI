using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAPI.Domain
{
    public class OrderItem
    {
        [Required]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }

        public Order Order { get; set; } = new();
        public Product Product { get; set; } = new();
    }
}
