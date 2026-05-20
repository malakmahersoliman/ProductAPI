using MediatR;
using ProductAPI.DTOs;
using ProductAPI.Services;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace ProductAPI.Feature.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public Task<LoginResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            const string adminEmail = "admin@test.com";
            const string adminPassword = "123456";
            const string adminRole = "SuperAdmin";

            if (request.Email != adminEmail || request.Password != adminPassword)
            {
                return Task.FromResult<LoginResponseDto?>(null);
            }

            var token = _jwtService.GenerateToken(adminEmail, adminRole);

            var response = new LoginResponseDto
            {
                Token = token,
                Email = adminEmail,
                Role = adminRole
            };

            return Task.FromResult<LoginResponseDto?>(response);
        }
    }
}