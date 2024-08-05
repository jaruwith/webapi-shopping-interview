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

        
    }
}