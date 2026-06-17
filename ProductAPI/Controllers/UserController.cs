using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs.User;
using ProductAPI.Feature.Users.Commands.CreateUser;
using ProductAPI.Feature.Users.Queries.GetAllUsers;
using ProductAPI.Feature.Users.Queries.GetUserById;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create(
            CreateUserRequestDto request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };

            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    
    [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAll(
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponseDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetUserByIdQuery { Id = id },
                cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
