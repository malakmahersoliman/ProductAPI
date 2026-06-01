using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs.Categories;
using ProductAPI.Feature.Categories.Commands.CreateCategory;
using ProductAPI.Feature.Categories.Queries.GetAllCategories;


namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateCategory(CategoryRequestDto dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));
            return CreatedAtAction(nameof(GetAllCategories), new { id = result.Id }, result);
        }
    }
}