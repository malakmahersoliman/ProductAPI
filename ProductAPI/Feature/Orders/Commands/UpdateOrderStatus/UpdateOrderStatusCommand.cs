using MediatR;
using ProductAPI.DTOs.Orders;

namespace ProductAPI.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<bool>
{
    public int Id { get; }
    public UpdateOrderStatusDto Dto { get; }

    public UpdateOrderStatusCommand(int id, UpdateOrderStatusDto dto)
    {
        Id = id;
        Dto = dto;
    }
}