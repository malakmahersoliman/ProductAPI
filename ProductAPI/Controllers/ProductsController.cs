using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs;

using ProductAPI.Feature.Products.Commands.CreateProduct;
using ProductAPI.Feature.Products.Commands.DeleteProduct;
using ProductAPI.Feature.Products.Commands.UpdateProduct;
using ProductAPI.Feature.Products.Queries.GetAllProducts;

using ProductAPI.Features.Products.Queries.GetProductById;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = await _mediator.Send(new CreateProductCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var updated = await _mediator.Send(new UpdateProductCommand(id, dto));

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteProductCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}