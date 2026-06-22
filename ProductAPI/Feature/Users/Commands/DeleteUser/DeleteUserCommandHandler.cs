using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;

namespace ProductAPI.Feature.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler
        : IRequestHandler<DeleteUserCommand, DeleteUserResult>
    {
        private readonly AppDbContext _context;

        public DeleteUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteUserResult> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (user is null)
            {
                return DeleteUserResult.NotFound;
            }

            var hasOrders = await _context.Orders
                .AnyAsync(o => o.CreatedById == request.Id, cancellationToken);

            if (hasOrders)
            {
                return DeleteUserResult.Conflict;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            return DeleteUserResult.Deleted;
        }
    }
}
