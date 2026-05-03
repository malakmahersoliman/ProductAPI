using MediatR;
using ProductAPI.Data;

namespace ProductAPI.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateCustomerCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(
            new object[] { request.Id },
            cancellationToken);

        if (customer == null)
            return false;

        customer.Name = request.Dto.Name;
        customer.Email = request.Dto.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}