namespace ProductAPI.services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Email { get; }
        string Role { get; }
        bool IsSuperAdmin { get; }
    }
}
