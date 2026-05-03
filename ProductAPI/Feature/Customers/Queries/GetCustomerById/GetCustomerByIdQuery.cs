using MediatR;
using ProductAPI.DTOs.Customers;

namespace ProductAPI.Feature.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerResponseDto?>
    {
        public int Id { get; }
        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
