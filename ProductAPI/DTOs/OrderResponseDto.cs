namespace ProductAPI.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = "";


        public int TotalAmount { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; internal set; } = "";
        public List<OrderItemResponseDto> Items { get; internal set; } = new List<OrderItemResponseDto>();
    }
}
