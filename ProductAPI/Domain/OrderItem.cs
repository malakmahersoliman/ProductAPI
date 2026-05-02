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
        public Order? Order { get; set; } = null;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; } = null;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }



    }
}
