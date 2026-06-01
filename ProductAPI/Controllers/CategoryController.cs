using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs.Categories;
using ProductAPI.Feature.Categories.Command.DeleteCategory;
using ProductAPI.Feature.Categories.Commands.CreateCategory;
using ProductAPI.Feature.Categories.Commands.UpdateCategory;
using ProductAPI.Feature.Categories.Queries.GetAllCategories;
using ProductAPI.Feature.Categories.Queries.GetCategoryById;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateCategory(CategoryRequestDto dto)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(dto));

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = result.Id },
                result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            CategoryRequestDto dto)
        {
            var result = await _mediator.Send(new UpdateCategoryCommand(id, dto));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _mediator.Send(new DeleteCategoryCommand(id));

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}