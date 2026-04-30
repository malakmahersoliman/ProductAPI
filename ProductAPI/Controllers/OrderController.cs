using Microsoft.AspNetCore.Mvc;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var OrderofCustomer = await _context.Orders
                .Include(c => c.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    CustomerName = o.Customer.Name,
                    Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })

                .ToListAsync();

            return Ok(OrderofCustomer);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Include(c => c.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    CustomerName = o.Customer.Name,
                    Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return BadRequest("Invalid CustomerId");

            var newOrder = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = 0,
                CustomerId = dto.CustomerId,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()


            };
            newOrder.TotalAmount = newOrder.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, new { newOrder.Id });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                return NotFound();
            order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        }
}

