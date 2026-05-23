using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs.Orders;
using ProductAPI.Feature.Orders.Commands.CreateOrder;
using ProductAPI.Feature.Orders.Queries.GetAllOrders;
using ProductAPI.Feature.Orders.Queries.GetOrderById;
using ProductAPI.Features.Orders.Commands.DeleteOrder;
using ProductAPI.Features.Orders.Commands.UpdateOrderStatus;


namespace ProductAPI.Controllers;

[Authorize(Roles = "SuperAdmin")]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var order = await _mediator.Send(new CreateOrderCommand(dto));

        if (order == null)
            return BadRequest("Invalid CustomerId or ProductId.");

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        var updated = await _mediator.Send(new UpdateOrderStatusCommand(id, dto));

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteOrderCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}