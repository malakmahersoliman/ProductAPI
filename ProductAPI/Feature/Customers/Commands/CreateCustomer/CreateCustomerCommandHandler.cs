using MediatR;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Feature.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponseDto>
    {
        private readonly AppDbContext _context;

        public CreateCustomerCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CustomerResponseDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
           var customer = new Customer
            {
                Name = request.Dto.Name,
                Email = request.Dto.Email
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
        }
    }
}
