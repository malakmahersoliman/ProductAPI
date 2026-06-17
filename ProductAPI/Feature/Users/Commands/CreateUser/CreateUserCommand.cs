using MediatR;
using ProductAPI.DTOs.User;

namespace ProductAPI.Feature.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserResponseDto>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
