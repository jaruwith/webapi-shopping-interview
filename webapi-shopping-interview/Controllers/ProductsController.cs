using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;
using webapi_shopping_interview.Data;

namespace webapi_shopping_interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Fetching all products and their stocks from the database.");
            var products = await _context.Products
                                         .Include(p => p.Stock)
                                         .ToListAsync();
            return Ok(products);
        }







        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation($"Fetching product with id {id} and its stock.");
            var product = await _context.Products
                                        .Include(p => p.Stock)
                                        .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

       
        //ใช้ อัพเดท stock
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] int stock)
        {
            _logger.LogInformation($"Updating stock for product with id {id}.");
            var contextStock = await _context.Stocks.Include(p => p.Product).FirstOrDefaultAsync(p => p.ProductId == id);

            if (contextStock == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                return NotFound();
            }

            contextStock.Quantity= stock;

            _context.Entry(contextStock).State = EntityState.Modified;

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
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
