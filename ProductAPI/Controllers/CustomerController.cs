using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.DTOs.Customers;
using ProductAPI.Feature.Customers.Commands.CreateCustomer;
using ProductAPI.Feature.Customers.Commands.DeleteCustomer;
using ProductAPI.Feature.Customers.Queries.GetAllCustomers;
using ProductAPI.Feature.Customers.Queries.GetCustomerById;
using ProductAPI.Features.Customers.Commands.UpdateCustomer;


namespace ProductAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _mediator.Send(new GetAllCustomersQuery());
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery(id));

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var customer = await _mediator.Send(new CreateCustomerCommand(dto));

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        var updated = await _mediator.Send(new UpdateCustomerCommand(id, dto));

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteCustomerCommand(id));

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}