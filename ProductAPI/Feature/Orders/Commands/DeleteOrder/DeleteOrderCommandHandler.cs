using MediatR;
using ProductAPI.Data;

namespace ProductAPI.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler
    : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteOrderCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(
            new object[] { request.Id },
            cancellationToken);

        if (order == null)
            return false;

        _context.Orders.Remove(order);
        //to-do: not handled to send error when deleting customer how has order
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}