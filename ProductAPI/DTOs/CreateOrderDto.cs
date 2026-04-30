using System.ComponentModel.DataAnnotations;

namespace ProductAPI.DTOs
{
    public class CreateOrderDto
    {
        public DateTime OrderDate { get; set; }

        [Required]

        public string Status { get; set; } = "";


        public int TotalAmount { get; set; }

        public int CustomerId { get; set; }
        public IEnumerable<CreateOrderItemDto> Items { get;  set; } = new List<CreateOrderItemDto>();
    }
}
