using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(DbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ดึงข้อมูล order 
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            _logger.LogInformation("Fetching all orders.");
            var orders = await _context.Set<Order>().Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
            _logger.LogInformation($"Fetched {orders.Count} orders.");
            return orders;
        }

        // ดึงข้อมูล order detail
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            _logger.LogInformation($"Fetching order with ID: {id}");
            var order = await _context.Set<Order>().Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                _logger.LogWarning($"Order with ID: {id} not found.");
                return NotFound();
            }

            return order;
        }

        //ใช้สร้าง order 
        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _logger.LogInformation("Creating a new order.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order data.");
                return BadRequest(ModelState);
            }

            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Order with ID: {order.OrderId} created successfully.");
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }



        private bool OrderExists(int id)
        {
            return _context.Set<Order>().Any(e => e.OrderId == id);
        }
    }
}