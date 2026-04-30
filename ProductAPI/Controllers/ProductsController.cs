using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Domain;
using ProductAPI.DTOs;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsAvailable = p.IsAvailable
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsAvailable = p.IsAvailable
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var newProduct = new Product
            {
                Name = dto.Name,
                Category = dto.Category,
                Price = dto.Price,
                Stock = dto.Stock,
                IsAvailable = dto.IsAvailable
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Category = newProduct.Category,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                IsAvailable = newProduct.IsAvailable
            };

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, responseDto);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Price = product.Price,
                Stock = product.Stock,
                IsAvailable = product.IsAvailable
            };

            return Ok(responseDto);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}