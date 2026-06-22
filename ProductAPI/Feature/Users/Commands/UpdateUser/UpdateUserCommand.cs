using MediatR;
using ProductAPI.DTOs.User;

namespace ProductAPI.Feature.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UpdateUserResult>
    {
        public int Id { get; }
        public UpdateUserRequestDto Dto { get; }

        public UpdateUserCommand(int id, UpdateUserRequestDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}
