namespace ProductAPI.DTOs.Products
{
    public class ProductFilterRequestDto
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsAvailable { get; set; }

        public string? StockStatus { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "name";

        public string? SortDirection { get; set; } = "asc";
    }
}