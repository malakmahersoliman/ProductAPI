using MediatR;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<bool>
{
    public int Id { get; }
    public UpdateCustomerDto Dto { get; }

    public UpdateCustomerCommand(int id, UpdateCustomerDto dto)
    {
        Id = id;
        Dto = dto;
    }
}