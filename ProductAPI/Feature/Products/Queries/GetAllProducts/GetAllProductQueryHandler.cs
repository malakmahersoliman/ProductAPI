using MediatR;
using ProductAPI.Data;
using ProductAPI.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using ProductAPI.DTOs.Common;

namespace ProductAPI.Feature.Products.Queries.GetAllProducts
{
    public class GetAllProductQueryHandler
        : IRequestHandler<GetAllProductsQuery, PagedResultDto<ProductResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetAllProductQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<ProductResponseDto>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {           

            var filter = request.Filter;

            var pageNumber = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var query = _context.Products
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();

                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    p.Category.Name.Contains(search));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }
            if (filter.IsAvailable.HasValue)
            {
                query = filter.IsAvailable.Value
                    ? query.Where(p => p.IsAvailable && p.Stock > 0)
                    : query.Where(p => !p.IsAvailable || p.Stock == 0);
            }
            if (!string.IsNullOrWhiteSpace(filter.StockStatus))
            {
                var stockStatus = filter.StockStatus.Trim().ToLowerInvariant();
                query = stockStatus switch
                {
                    "instock" => query.Where(p => p.IsAvailable && p.Stock > 0),
                    "lowstock" => query.Where(p => p.IsAvailable && p.Stock > 0 && p.Stock <= 4),
                    "outofstock" => query.Where(p => !p.IsAvailable || p.Stock == 0),
                    _ => query
                };
            }

            query = ApplySorting(query, filter.SortBy, filter.SortDirection);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsAvailable = p.IsAvailable,
                    ImagePath=p.ImagePath    
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<ProductResponseDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        // Helper method to
        // apply sorting based on the provided sortBy and sortDirection

        private static IQueryable<Domain.Product> ApplySorting(
            IQueryable<Domain.Product> query,
            string? sortBy,
            string? sortDirection)
        {
            var descending = sortDirection?.ToLower() == "desc";

            return sortBy?.ToLower() switch
            {
                "price" => descending
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),

                "stock" => descending
                    ? query.OrderByDescending(p => p.Stock)
                    : query.OrderBy(p => p.Stock),

                "category" => descending
                    ? query.OrderByDescending(p => p.Category.Name)
                    : query.OrderBy(p => p.Category.Name),

                "name" or _ => descending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
            };
        }
    }
}