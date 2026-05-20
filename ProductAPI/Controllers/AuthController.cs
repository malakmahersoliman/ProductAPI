using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs;
using ProductAPI.Feature.Auth.Commands.Login;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var command = new LoginCommand(request.Email, request.Password);

            var response = await _mediator.Send(command);

            if (response is null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid email or password."
                });
            }

            return Ok(response);
        }
    }
}