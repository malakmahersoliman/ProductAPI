using Microsoft.AspNetCore.Http;
using ProductAPI.services;
using System.Security.Claims;

namespace ProductAPI.Services
{

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return int.TryParse(userId, out var id) ? id : 0;
            }
        }

        public string Email
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            }
        }

        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            }
        }

        public bool IsSuperAdmin => Role == "SuperAdmin";
    }
}