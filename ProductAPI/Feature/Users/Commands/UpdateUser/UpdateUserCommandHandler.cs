using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.Services;

namespace ProductAPI.Feature.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler
        : IRequestHandler<UpdateUserCommand, UpdateUserResult>
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UpdateUserCommandHandler(
            AppDbContext context,
            IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<UpdateUserResult> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (user is null)
            {
                return UpdateUserResult.NotFound;
            }

            var emailTaken = await _context.Users
                .AnyAsync(
                    u => u.Email == request.Dto.Email && u.Id != request.Id,
                    cancellationToken);

            if (emailTaken)
            {
                return UpdateUserResult.EmailConflict;
            }

            if (!Enum.TryParse<UserRole>(request.Dto.Role, true, out var role))
            {
                return UpdateUserResult.InvalidRole;
            }

            user.Email = request.Dto.Email;
            user.Role = role;

            if (!string.IsNullOrWhiteSpace(request.Dto.Password))
            {
                user.PasswordHash = _passwordService.HashPassword(request.Dto.Password);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return UpdateUserResult.Updated;
        }
    }
}
