using MediatR;
using ProductAPI.DTOs.User;


namespace ProductAPI.Feature.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserResponseDto>>
    {
    }
}