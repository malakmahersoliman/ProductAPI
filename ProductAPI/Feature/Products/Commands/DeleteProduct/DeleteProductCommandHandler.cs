using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;

namespace ProductAPI.Feature.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler
        : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly AppDbContext _context;

        public DeleteProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteProductResult> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                return DeleteProductResult.NotFound;
            }

            var isUsedInOrders = await _context.OrderItems
                .AnyAsync(oi => oi.ProductId == request.Id, cancellationToken);

            if (isUsedInOrders)
            {
                return DeleteProductResult.Conflict;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return DeleteProductResult.Deleted;
        }
    }
}