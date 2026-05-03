using MediatR;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Feature.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<CustomerResponseDto>
    {
        public CreateCustomerDto Dto { get; }
        public CreateCustomerCommand(CreateCustomerDto dto)
        {
            Dto = dto;
        }
    }
}
