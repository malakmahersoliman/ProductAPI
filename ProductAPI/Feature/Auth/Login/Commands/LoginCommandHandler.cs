using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.Auth;
using ProductAPI.Services;

namespace ProductAPI.Feature.Auth.Login.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto?>
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public LoginCommandHandler(
            AppDbContext context,
            IJwtService jwtService,
            IPasswordService passwordService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<LoginResponseDto?> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                return null;
            }

            var isPasswordValid = _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user.Email, user.Role);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}