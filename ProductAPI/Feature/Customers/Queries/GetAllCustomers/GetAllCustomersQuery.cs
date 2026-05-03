using MediatR;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Feature.Customers.Queries.GetAllCustomers
{
    public class GetAllCustomersQuery : IRequest<List<CustomerResponseDto>>
    {
    }
}
