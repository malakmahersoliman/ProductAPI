using MediatR;
using ProductAPI.Feature.Products.Commands.DeleteProduct;

namespace ProductAPI.Feature.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeleteProductResult>
    {
        public int Id { get; set; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
