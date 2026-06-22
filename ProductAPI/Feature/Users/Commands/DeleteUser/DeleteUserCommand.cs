using MediatR;

namespace ProductAPI.Feature.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<DeleteUserResult>
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
