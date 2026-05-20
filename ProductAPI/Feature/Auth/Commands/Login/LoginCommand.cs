using MediatR;
using ProductAPI.DTOs;

namespace ProductAPI.Feature.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginCommand(string email , string password) 
        { 
            Email = email;
            Password = password;
        }

    }
}
