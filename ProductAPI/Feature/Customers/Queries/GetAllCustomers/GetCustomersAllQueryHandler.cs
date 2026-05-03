using MediatR;
using ProductAPI.Data;
using ProductAPI.DTOs.Customers;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Feature.Customers.Queries.GetAllCustomers
{
    public class GetCustomersAllQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerResponseDto>>
    {

        private readonly AppDbContext _context;

        public GetCustomersAllQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerResponseDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email
            })
            .ToListAsync(cancellationToken);
        }
    }
}
