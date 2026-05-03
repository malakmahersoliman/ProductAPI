using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Feature.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDto?>
    {
        private readonly AppDbContext _context;
        public GetCustomerByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomerResponseDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .Where(c => c.Id == request.Id)
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
