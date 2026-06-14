namespace ProductAPI.Domain
{
    public class Payment
    {
        public int Id { get; set; } 

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "EGP";

        public string Method { get; set; } = string.Empty;

        public string Status { get; set; } = "Succeeded";
        //so for now succeeded

        public string Provider { get; set; } = "Manual";
        //later when using payment gateway we can change it to be the name of the gateway


        public string? ProviderPaymentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }
    }
}
