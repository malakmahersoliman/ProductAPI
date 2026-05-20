namespace ProductAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(string email, string role);
    }
}