using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketReasonController : ControllerBase
    {
        private readonly EduPlusDbContext _context;

        public TicketReasonController(EduPlusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null, [FromQuery] string? sortBy = "updated", [FromQuery] bool sortDesc = true, [FromQuery] bool showInactive = false)
        {
            var query = _context.TicketReasons.AsQueryable();

            query = query.Where(r => r.IsActive == !showInactive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                "created" or "createdat" => sortDesc ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
                _ => sortDesc ? query.OrderByDescending(r => r.UpdatedAt) : query.OrderBy(r => r.UpdatedAt)
            };

            var result = await query
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.IsActive,
                    r.CreatedAt,
                    r.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == r.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var reasons = await _context.TicketReasons
                .Where(r => r.IsActive)
                .OrderBy(r => r.Id)
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            return Ok(reasons);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TicketReason dto)
        {
            if (await _context.TicketReasons.AnyAsync(r => r.Name == dto.Name && r.IsActive))
                return BadRequest(new { message = "Taki powód zgłoszenia już istnieje" });

            var reason = new TicketReason
            {
                Name = dto.Name,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.TicketReasons.Add(reason);
            await _context.SaveChangesAsync();

            return Ok(reason);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] TicketReason dto)
        {
            var reason = await _context.TicketReasons.FindAsync(id);
            if (reason == null) return NotFound();

            if (await _context.TicketReasons.AnyAsync(r => r.Name == dto.Name && r.Id != id && r.IsActive))
                return BadRequest(new { message = "Taki powód zgłoszenia już istnieje" });

            reason.Name = dto.Name;
            reason.IsActive = dto.IsActive;
            reason.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(reason);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var reason = await _context.TicketReasons.FindAsync(id);
            if (reason == null) return NotFound();

            _context.TicketReasons.Remove(reason);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

