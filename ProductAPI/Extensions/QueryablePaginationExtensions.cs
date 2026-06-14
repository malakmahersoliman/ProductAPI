namespace ProductAPI.Extensions
{
    public static class QueryablePaginationExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}