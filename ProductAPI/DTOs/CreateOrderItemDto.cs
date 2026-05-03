using ProductAPI.Domain;
using System.ComponentModel.DataAnnotations;


namespace ProductAPI.DTOs
{
    public class CreateOrderItemDto
    {


        //public int Id { get; set; } as this is controled by database


        //public int OrderId { get; set; } ef core after creating order

        [Required]
        public int ProductId { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        //public int UnitPrice { get; set; } backend from product table 
    }
}
