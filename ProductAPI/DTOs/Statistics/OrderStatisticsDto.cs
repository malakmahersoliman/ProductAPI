namespace ProductAPI.DTOs.Statistics
{
    public class OrderStatisticsDto
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
        public decimal TodaySales { get; set; }
    }
}
