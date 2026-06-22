using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain;
using ProductAPI.Services;

namespace ProductAPI.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordService passwordService)
    {
        await context.Database.MigrateAsync();
        await ClearDataAsync(context);

        var now = DateTime.UtcNow;
        var today = now.Date;

        var admin = new User
        {
            Email = "admin@store.com",
            PasswordHash = passwordService.HashPassword("Admin123!"),
            Role = UserRole.SuperAdmin,
            CreatedAt = now.AddMonths(-6),
        };

        var staff = new User
        {
            Email = "staff@store.com",
            PasswordHash = passwordService.HashPassword("Staff123!"),
            Role = UserRole.User,
            CreatedAt = now.AddMonths(-3),
        };

        context.Users.AddRange(admin, staff);
        await context.SaveChangesAsync();

        var categories = new[]
        {
            new Category { Name = "Beverages", Description = "Soft drinks, juices, and water" },
            new Category { Name = "Snacks", Description = "Chips, biscuits, and quick bites" },
            new Category { Name = "Dairy", Description = "Milk, cheese, and yogurt" },
            new Category { Name = "Bakery", Description = "Bread and baked goods" },
            new Category { Name = "Household", Description = "Cleaning and daily essentials" },
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var beverages = categories[0];
        var snacks = categories[1];
        var dairy = categories[2];
        var bakery = categories[3];
        var household = categories[4];

        var products = new[]
        {
            new Product { Name = "Coca-Cola 330ml", CategoryId = beverages.Id, Price = 15m, Stock = 42, IsAvailable = true },
            new Product { Name = "Orange Juice 1L", CategoryId = beverages.Id, Price = 35m, Stock = 18, IsAvailable = true },
            new Product { Name = "Sparkling Water 500ml", CategoryId = beverages.Id, Price = 12m, Stock = 3, IsAvailable = true },
            new Product { Name = "Potato Chips 150g", CategoryId = snacks.Id, Price = 18m, Stock = 24, IsAvailable = true },
            new Product { Name = "Chocolate Cookies 200g", CategoryId = snacks.Id, Price = 25m, Stock = 4, IsAvailable = true },
            new Product { Name = "Mixed Nuts 100g", CategoryId = snacks.Id, Price = 45m, Stock = 0, IsAvailable = false },
            new Product { Name = "Full Cream Milk 1L", CategoryId = dairy.Id, Price = 28m, Stock = 14, IsAvailable = true },
            new Product { Name = "Cheddar Cheese 200g", CategoryId = dairy.Id, Price = 55m, Stock = 2, IsAvailable = true },
            new Product { Name = "Greek Yogurt 500g", CategoryId = dairy.Id, Price = 30m, Stock = 0, IsAvailable = true },
            new Product { Name = "White Bread Loaf", CategoryId = bakery.Id, Price = 14m, Stock = 16, IsAvailable = true },
            new Product { Name = "Croissant Pack (4)", CategoryId = bakery.Id, Price = 32m, Stock = 6, IsAvailable = true },
            new Product { Name = "Dish Soap 500ml", CategoryId = household.Id, Price = 22m, Stock = 10, IsAvailable = true },
            new Product { Name = "Paper Towels Roll", CategoryId = household.Id, Price = 19m, Stock = 1, IsAvailable = true },
            new Product { Name = "Floor Cleaner 750ml", CategoryId = household.Id, Price = 38m, Stock = 7, IsAvailable = true },
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var customers = new[]
        {
            new Customer { Name = "Ahmed Hassan", Email = "ahmed.hassan@email.com" },
            new Customer { Name = "Sara Mohamed", Email = "sara.mohamed@email.com" },
            new Customer { Name = "Omar Ali", Email = "omar.ali@email.com" },
            new Customer { Name = "Layla Ibrahim", Email = "layla.ibrahim@email.com" },
            new Customer { Name = "Mahmoud Farouk", Email = "mahmoud.farouk@email.com" },
            new Customer { Name = "Nour El-Din", Email = "nour.eldin@email.com" },
        };

        context.Customers.AddRange(customers);
        await context.SaveChangesAsync();

        var cola = products[0];
        var juice = products[1];
        var chips = products[3];
        var cookies = products[4];
        var milk = products[6];
        var cheese = products[7];
        var bread = products[9];
        var croissant = products[10];
        var dishSoap = products[11];
        var paperTowels = products[12];
        var floorCleaner = products[13];

        var ahmed = customers[0];
        var sara = customers[1];
        var omar = customers[2];
        var layla = customers[3];
        var mahmoud = customers[4];
        var nour = customers[5];

        var orders = new List<Order>
        {
            BuildOrder(
                ahmed, staff.Id, today.AddHours(10), OrderStatus.Completed, PaymentStatus.Paid, "Cash",
                new[] { (cola, 2), (croissant, 1) }),
            BuildOrder(
                sara, admin.Id, today.AddHours(14).AddMinutes(20), OrderStatus.Completed, PaymentStatus.Paid, "Card",
                new[] { (milk, 1), (bread, 2) }),
            BuildOrder(
                omar, staff.Id, today.AddHours(16), OrderStatus.Completed, PaymentStatus.Paid, "Cash",
                new[] { (floorCleaner, 1) }),
            BuildOrder(
                omar, staff.Id, today.AddDays(-1).AddHours(11), OrderStatus.Completed, PaymentStatus.Paid, "Card",
                new[] { (chips, 1), (juice, 1) }),
            BuildOrder(
                layla, admin.Id, today.AddDays(-5).AddHours(9), OrderStatus.Completed, PaymentStatus.Paid, "Cash",
                new[] { (cheese, 1) }),
            BuildOrder(
                mahmoud, staff.Id, today.AddHours(12), OrderStatus.Pending, PaymentStatus.Unpaid, "Cash",
                new[] { (dishSoap, 1), (cola, 3) }),
            BuildOrder(
                nour, staff.Id, today.AddDays(-2).AddHours(15), OrderStatus.Pending, PaymentStatus.Unpaid, "Card",
                new[] { (paperTowels, 1) }),
            BuildOrder(
                ahmed, admin.Id, today.AddDays(-3).AddHours(13), OrderStatus.Cancelled, PaymentStatus.Unpaid, "Card",
                new[] { (cookies, 2) }),
        };

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();
    }

    private static async Task ClearDataAsync(AppDbContext context)
    {
        await context.Payments.ExecuteDeleteAsync();
        await context.OrderItems.ExecuteDeleteAsync();
        await context.Orders.ExecuteDeleteAsync();
        await context.Products.ExecuteDeleteAsync();
        await context.Customers.ExecuteDeleteAsync();
        await context.Categories.ExecuteDeleteAsync();
        await context.Users.ExecuteDeleteAsync();
    }

    private static Order BuildOrder(
        Customer customer,
        int createdById,
        DateTime createdAt,
        OrderStatus status,
        PaymentStatus paymentStatus,
        string paymentMethod,
        (Product product, int quantity)[] items)
    {
        var orderItems = items.Select(item => new OrderItem
        {
            ProductId = item.product.Id,
            Quantity = item.quantity,
            UnitPrice = item.product.Price,
        }).ToList();

        var totalAmount = orderItems.Sum(item => item.UnitPrice * item.Quantity);

        var order = new Order
        {
            CustomerId = customer.Id,
            CreatedById = createdById,
            OrderDate = createdAt,
            CreatedAt = createdAt,
            Status = status,
            PaymentStatus = paymentStatus,
            TotalAmount = totalAmount,
            OrderItems = orderItems,
        };

        order.Payments.Add(new Payment
        {
            Amount = totalAmount,
            Currency = "EGP",
            Method = paymentMethod,
            Status = paymentStatus == PaymentStatus.Paid ? PaymentStatus.Paid : PaymentStatus.Unpaid,
            Provider = "Manual",
            CreatedAt = createdAt,
            CompletedAt = paymentStatus == PaymentStatus.Paid ? createdAt : null,
        });

        return order;
    }
}
