namespace ProductAPI.Domain
{
    public class Order
    {
      public int Id { get; set; }

     public DateTime OrderDate { get; set; }

     public OrderStatus Status { get; set; } //change to enum 
     

     public decimal TotalAmount { get; set; }

     public int CustomerId { get; set; }
     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     public Customer? Customer { get; set; } = null;

     public string PaymentStatus { get; set; } = "Unpaid";
     public ICollection<Payment> Payments { get; set; } = new List<Payment>();
     public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
