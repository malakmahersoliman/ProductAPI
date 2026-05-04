using MediatR;
using ProductAPI.Data;

namespace ProductAPI.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler
    : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateOrderStatusCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync(request.Id  );

        if (order == null)
            return false;

        order.Status = request.Dto.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}