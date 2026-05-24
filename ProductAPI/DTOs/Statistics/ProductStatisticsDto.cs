namespace ProductAPI.DTOs.Statistics
{
    public class ProductStatisticsDto
    {
        public int Total { get; set; }
        public int Available { get; set; }
        public int OutOfStock { get; set; }
        public int LowStock { get; set; }
    }
}
