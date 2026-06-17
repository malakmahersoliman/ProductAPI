using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.DTOs.User;


namespace ProductAPI.Feature.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, List<UserResponseDto>>
    {
        private readonly AppDbContext _context;

        public GetAllUsersQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponseDto>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}