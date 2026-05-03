using MediatR;
using ProductAPI.Data;

namespace ProductAPI.Feature.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeleteCustomerCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _context.Customers.Find(request.Id);
            if (customer == null)
            {
                return false;
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
