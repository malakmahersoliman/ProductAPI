using ProductAPI.Domain;
using System.ComponentModel.DataAnnotations;


namespace ProductAPI.DTOs
{
    public class CreateOrderItemDto
    {

        [Required]
        public int Id { get; set; }

     
        public int OrderId { get; set; }

       
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }
    }
}
