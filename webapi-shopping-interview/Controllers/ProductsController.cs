using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(DbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ใช้ดึงข้อมูล Products
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Fetching all products from the database.");
            return await _context.Set<Product>().ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation($"Fetching product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            return product;
        }

        // ใช้สร้าง product
        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] Product product, IFormFile image)
        {
            _logger.LogInformation("Creating a new product.");
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }
            }

            _context.Set<Product>().Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product with id {product.ProductId} created.");

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        //ใช้ อัพเดท stock
        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] int stock)
        {
            _logger.LogInformation($"Updating stock for product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            product.Stock = stock;
            var thailandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var thailandTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, thailandTimeZone);

            product.UpdatedAt = thailandTime;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Stock for product with id {id} updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    _logger.LogError($"Product with id {id} not found during stock update.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Concurrency exception while updating stock for product with id {id}.");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deleting product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            _context.Set<Product>().Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Product with id {id} deleted.");

            return NoContent();
        }

        // GET: api/Products/images/5
        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetProductImage(int id)
        {
            _logger.LogInformation($"Fetching image for product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);
            if (product == null || product.Image == null)
            {
                _logger.LogWarning($"Product with id {id} or its image not found.");
                return NotFound();
            }

            return File(product.Image, "image/jpeg");
        }

        // POST: api/Products/5/upload
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            _logger.LogInformation($"Uploading image for product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    product.Image = memoryStream.ToArray();
                }

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Image for product with id {id} uploaded.");
            }

            return NoContent();
        }

        // GET: api/Products/5/image
        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImage(int id)
        {
            _logger.LogInformation($"Fetching image for product with id {id}.");
            var product = await _context.Set<Product>().FindAsync(id);
            if (product == null || product.Image == null)
            {
                _logger.LogWarning($"Product with id {id} or its image not found.");
                return NotFound();
            }

            return File(product.Image, "image/jpeg");
        }

        private bool ProductExists(int id)
        {
            return _context.Set<Product>().Any(e => e.ProductId == id);
        }
    }
}