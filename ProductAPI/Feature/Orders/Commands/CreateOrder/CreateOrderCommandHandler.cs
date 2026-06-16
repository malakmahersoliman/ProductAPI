using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs.Orders;
using ProductAPI.Feature.Orders.Commands.CreateOrder;
using ProductAPI.services;

namespace ProductAPI.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderResponseDto?>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateOrderCommandHandler(
            AppDbContext context,
          ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;

    }

    public async Task<OrderResponseDto?> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var currentUser = _currentUser.GetCurrentUser();

        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == dto.CustomerId, cancellationToken);

        if (!customerExists)
        {
            return null;
        }

        var groupedItems = dto.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        if (groupedItems.Any(i => i.Quantity <= 0))
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        var productIds = groupedItems
            .Select(i => i.ProductId)
            .ToList();

        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (products.Count != productIds.Count)
        {
            return null;
        }

        foreach (var item in groupedItems)
        {
            var product = products.First(p => p.Id == item.ProductId);

            if (!product.IsAvailable)
            {
                throw new InvalidOperationException($"{product.Name} is not available.");
            }

            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for {product.Name}. Available stock: {product.Stock}."
                );
            }
        }

        var totalAmount = groupedItems.Sum(item =>
        {
            var product = products.First(p => p.Id == item.ProductId);
            return product.Price * item.Quantity;
        });

        if (!dto.PaymentShouldSucceed)
        {
            var reason = string.IsNullOrWhiteSpace(dto.PaymentFailureReason)
                ? "Payment failed. Please try another payment method."
                : dto.PaymentFailureReason;

            throw new InvalidOperationException(reason);
        }

        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
                CreatedAt=DateTime.UtcNow,
                CreatedById=currentUser.UserId,
                TotalAmount = totalAmount
            };


            foreach (var item in groupedItems)
            {
                var product = products.First(p => p.Id == item.ProductId);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                product.Stock -= item.Quantity;
            }

            var payment = new Payment
            {
                Order = order,
                Amount = totalAmount,
                Currency = "EGP",
                Method = dto.PaymentMethod,
                Status = PaymentStatus.Unpaid,
                Provider = "Manual",
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            _context.Payments.Add(payment);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Id == order.Id)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status.ToString(),
                    PaymentStatus = o.PaymentStatus.ToString(),
                    TotalAmount = o.TotalAmount,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    CreatedById = o.CreatedById,
                    CreatedByEmail = o.CreatedBy!.Email,
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
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}