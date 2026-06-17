using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.User;

namespace ProductAPI.Feature.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, UserResponseDto>
    {
        private readonly AppDbContext _context;

        public CreateUserCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                throw new Exception("Email already exists");
            }

            if (!Enum.TryParse<UserRole>(request.Role, true, out var role))
            {
                throw new Exception("Invalid role");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
