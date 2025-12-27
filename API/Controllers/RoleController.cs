using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public RoleController(SchoolDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Roles.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _context.Roles.FindAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(Role entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Roles.FindAsync(id);
            if (item == null) return NotFound();
            item.IsActive = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
