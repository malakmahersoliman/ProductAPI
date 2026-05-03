using MediatR;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Feature.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<OrderResponseDto>
    {
        public CreateOrderDto Dto { get; }

        public CreateOrderCommand(CreateOrderDto dto)
        {
            Dto = dto;
        }
    }
}
