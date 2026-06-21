using MediatR;

namespace ProductAPI.Feature.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
    }
}
