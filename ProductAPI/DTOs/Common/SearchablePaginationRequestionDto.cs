namespace ProductAPI.DTOs.Common
{
    public class SearchablePaginationRequestionDto : PaginationRequestDto
    {
        public string? Search { get; set; }
    }
}
