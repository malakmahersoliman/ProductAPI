

namespace ProductAPI.DTOs
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = "";


        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }
    }
}
