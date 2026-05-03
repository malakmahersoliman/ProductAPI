using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.Orders;
using ProductAPI.Feature.Orders.Commands.CreateOrder;

namespace ProductAPI.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderResponseDto?>
{
    private readonly AppDbContext _context;

    public CreateOrderCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto?> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == dto.CustomerId, cancellationToken);

        if (!customerExists)
            return null;

        var productIds = dto.Items.Select(i => i.ProductId).ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != productIds.Distinct().Count())
            return null;

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        foreach (var item in dto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            order.OrderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.OrderItems
            .Sum(oi => oi.Quantity * oi.UnitPrice);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.Id == order.Id)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Subtotal = oi.Quantity * oi.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}