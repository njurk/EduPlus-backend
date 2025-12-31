using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public RoleController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Roles
            .AsNoTracking()
            .ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Role entity)
    {
        _context.Roles.Add(entity);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Role entity)
    {
        if (id != entity.Id) return BadRequest();

        var dbRole = await _context.Roles.FindAsync(id);
        if (dbRole == null) return NotFound();

        dbRole.Name = entity.Name;
        dbRole.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(dbRole);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Roles.FindAsync(id);
        if (item == null) return NotFound();

        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}