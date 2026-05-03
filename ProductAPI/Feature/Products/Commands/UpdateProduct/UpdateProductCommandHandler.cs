using MediatR;
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
            product.Name = request.Dto.Name;
            product.Category = request.Dto.Category;
            product.Price = request.Dto.Price;
            product.Stock = request.Dto.Stock;
            product.IsAvailable = request.Dto.IsAvailable;
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
