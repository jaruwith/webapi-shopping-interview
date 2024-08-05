using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(DbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //ใช้ login
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<User>> GetUser(string username, string passwordHash)
        {
            _logger.LogInformation($"Fetching user with username: {username}.");

            var user = await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);

            if (user == null)
            {
                _logger.LogWarning($"User with username: {username} not found or password does not match.");
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation($"Fetching user with ID: {id}");
            var user = await _context.Set<User>().FindAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found.");
                return NotFound();
            }

            return user;
        }

        //ใช้สมัคร user
        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _logger.LogInformation("Creating a new user.");

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.PasswordHash) || string.IsNullOrWhiteSpace(user.Email))
            {
                _logger.LogWarning("Invalid user data. Username, PasswordHash, and Email are required.");
                return BadRequest(new { message = "Username, PasswordHash, and Email are required" });
            }

            var existingUser = await _context.Set<User>().FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null)
            {
                _logger.LogWarning($"User with username: {user.Username} already exists.");
                return Conflict(new { message = "User already exists" });
            }

            var thailandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var thailandTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, thailandTimeZone);
            user.CreatedAt = thailandTime;

            _context.Set<User>().Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with ID: {user.UserId} created successfully.");
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }


        private bool UserExists(int id)
        {
            return _context.Set<User>().Any(e => e.UserId == id);
        }
    }
}