using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly ILogger<OrderItemsController> _logger;

        public OrderItemsController(DbContext context, ILogger<OrderItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            _logger.LogInformation("Fetching all order items.");
            var orderItems = await _context.Set<OrderItem>().Include(oi => oi.Product).ToListAsync();
            _logger.LogInformation($"Fetched {orderItems.Count} order items.");
            return orderItems;
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            _logger.LogInformation($"Fetching order item with ID: {id}");
            var orderItem = await _context.Set<OrderItem>().Include(oi => oi.Product).FirstOrDefaultAsync(oi => oi.OrderItemId == id);

            if (orderItem == null)
            {
                _logger.LogWarning($"Order item with ID: {id} not found.");
                return NotFound();
            }

            return orderItem;
        }

        // POST: api/OrderItems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation("Creating a new order item.");
            _context.Set<OrderItem>().Add(orderItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order item with ID: {orderItem.OrderItemId} created successfully.");
            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.OrderItemId }, orderItem);
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                _logger.LogWarning($"Order item ID mismatch. ID in URL: {id}, ID in body: {orderItem.OrderItemId}");
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                {
                    _logger.LogWarning($"Order item with ID: {id} not found.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Concurrency error occurred while updating order item with ID: {id}");
                    throw;
                }
            }

            _logger.LogInformation($"Order item with ID: {orderItem.OrderItemId} updated successfully.");
            return NoContent();
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            _logger.LogInformation($"Deleting order item with ID: {id}");
            var orderItem = await _context.Set<OrderItem>().FindAsync(id);
            if (orderItem == null)
            {
                _logger.LogWarning($"Order item with ID: {id} not found.");
                return NotFound();
            }

            _context.Set<OrderItem>().Remove(orderItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order item with ID: {id} deleted successfully.");
            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.Set<OrderItem>().Any(e => e.OrderItemId == id);
        }
    }
}