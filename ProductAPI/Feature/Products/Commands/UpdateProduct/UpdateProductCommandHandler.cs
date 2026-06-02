using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;

namespace ProductAPI.Feature.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {

        private readonly AppDbContext _context;

        public UpdateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
           var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return false;
            }
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.Dto.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new InvalidOperationException("Category does not exist.");
            }
            product.Name = request.Dto.Name;
            product.CategoryId = request.Dto.CategoryId;
            product.Price = request.Dto.Price;
            product.Stock = request.Dto.Stock;
            product.IsAvailable = request.Dto.IsAvailable;
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
