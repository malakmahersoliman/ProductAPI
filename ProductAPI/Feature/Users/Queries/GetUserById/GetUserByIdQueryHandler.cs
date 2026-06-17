using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.User;

namespace ProductAPI.Feature.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, UserResponseDto?>
    {
        private readonly AppDbContext _context;

        public GetUserByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto?> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == request.Id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}