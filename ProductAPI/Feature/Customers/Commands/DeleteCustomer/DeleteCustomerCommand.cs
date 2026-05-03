using MediatR;

namespace ProductAPI.Feature.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int Id { get; }
        public DeleteCustomerCommand(int id)
        {
            Id = id;
        }
    }
}
