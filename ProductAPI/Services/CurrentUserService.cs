using ProductAPI.Common.Secuirty;
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

        public CurrentUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var userIdValue = user?.FindFirstValue(ClaimTypes.NameIdentifier);

            return new CurrentUser
            {
                UserId = int.TryParse(userIdValue, out var userId) ? userId : 0,
                Email = user?.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                Role = user?.FindFirstValue(ClaimTypes.Role) ?? string.Empty
            };
        }
    }
}