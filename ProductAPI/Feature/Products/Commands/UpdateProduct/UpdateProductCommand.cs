using MediatR;
using ProductAPI.DTOs;

namespace ProductAPI.Feature.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<bool>
    {

        public int Id { get; set; }
        public UpdateProductDto Dto { get; }

        public UpdateProductCommand(int id, UpdateProductDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
