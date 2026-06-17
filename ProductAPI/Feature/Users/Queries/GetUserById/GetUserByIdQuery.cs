using MediatR;
using ProductAPI.DTOs.User;

namespace ProductAPI.Feature.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserResponseDto>
    {
        public int Id { get; set; }
    }
}
