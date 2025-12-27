using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public UserController(SchoolDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Users.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _context.Users.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(User entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null) return NotFound();
            item.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
