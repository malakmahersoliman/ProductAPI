namespace ProductAPI.Domain
{
    public class Order
    {
      public int Id { get; set; }

     public DateTime OrderDate { get; set; }

     public string Status { get; set; } = "";
     

     public int TotalAmount { get; set; }

     public int CustomerId { get; set; }
      public Customer Customer { get; set; } = new();
      public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
