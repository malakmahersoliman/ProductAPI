namespace ProductAPI.DTOs.Common
{
    public class PaginationRequestDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } = "id";

        public string? SortDirection { get; set; } = "desc";
    }
}
