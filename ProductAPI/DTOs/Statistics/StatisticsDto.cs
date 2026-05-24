namespace ProductAPI.DTOs.Statistics
{
    public class StatisticsDto
    {
        public ProductStatisticsDto Products { get; set; } = new();
        public OrderStatisticsDto Orders { get; set; } = new();
        public CustomerStatisticsDto Customers { get; set; } = new();
        public UserStatisticsDto Users { get; set; } = new();
    }
}
