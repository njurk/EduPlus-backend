using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public RoleController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false, [FromQuery] bool showInactive = false)
    {
        var query = _context.Roles.AsNoTracking().AsQueryable();

        if (showInactive)
        {
            query = query.Where(r => !r.IsActive);
        }
        else
        {
            query = query.Where(r => r.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(r => r.Name.Contains(s));
        }

        query = sortBy?.ToLower() switch
        {
            "name" => sortDesc ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
            "created" => sortDesc ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt),
            "updated" => sortDesc ? query.OrderByDescending(r => r.UpdatedAt) : query.OrderBy(r => r.UpdatedAt),
            _ => sortDesc ? query.OrderByDescending(r => r.CreatedAt) : query.OrderBy(r => r.CreatedAt)
        };

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return role == null ? NotFound() : Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Role entity)
    {
        if (await _context.Roles.AnyAsync(r => r.Name == entity.Name))
            return BadRequest("Rola o tej nazwie już istnieje");

        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        entity.IsActive = true;

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
        dbRole.UpdatedAt = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Roles.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        return Ok(dbRole);
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var role = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return NotFound();

        role.IsActive = true;

        if (role.Name.EndsWith(" (nieaktywny)"))
        {
            role.Name = role.Name.Replace(" (nieaktywny)", "").Trim();
        }

        role.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Rola przywrócona", id = role.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Roles.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            if (!item.Name.EndsWith(" (nieaktywny)"))
            {
                item.Name += " (nieaktywny)";
            }
            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return BadRequest("Trwałe usuwanie ról jest zablokowane");
        }
    }
}