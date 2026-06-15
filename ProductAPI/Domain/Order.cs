namespace ProductAPI.Domain
{
    public class Order
    {
      public int Id { get; set; }

     public DateTime OrderDate { get; set; }

     public OrderStatus Status { get; set; } //change to enum 
     

     public decimal TotalAmount { get; set; }

     public int CustomerId { get; set; }
     public Customer? Customer { get; set; } = null;
     public PaymentStatus PaymentStatus { get; set; }
     public ICollection<Payment> Payments { get; set; } = new List<Payment>();
     public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
      public int CreatedById { get; set; }

      public User? CreatedBy { get; set; }
      public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
