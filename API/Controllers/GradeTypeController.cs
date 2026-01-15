using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeTypeController : ControllerBase
{
    private readonly EduPlusDbContext _context;

    public GradeTypeController(EduPlusDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false, [FromQuery] bool showInactive = false)
    {
        var query = _context.GradeTypes.AsNoTracking().AsQueryable();

        query = showInactive ? query.Where(r => !r.IsActive) : query.Where(r => r.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(x => x.Name.Contains(s));
        }

        query = sortBy?.ToLower() switch
        {
            "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "value" => sortDesc ? query.OrderByDescending(x => x.Numeric) : query.OrderBy(x => x.Numeric),
            "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
            _ => sortDesc ? query.OrderByDescending(x => x.Numeric) : query.OrderBy(x => x.Numeric)
        };

        var result = await query
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Numeric,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt,
                ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault() ?? "System"
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeType entity)
    {
        entity.IsActive = true;
        _context.GradeTypes.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeType entity)
    {
        if (id != entity.Id) return BadRequest();

        if (entity.IsActive && entity.Name.EndsWith(" (nieaktywny)"))
            entity.Name = entity.Name.Replace(" (nieaktywny)", "");

        _context.Entry(entity).State = EntityState.Modified;
        _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.GradeTypes.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.GradeTypes.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            item.IsActive = false;
            if (!item.Name.EndsWith(" (nieaktywny)"))
                item.Name += " (nieaktywny)";
        }
        else
        {
            _context.GradeTypes.Remove(item);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var item = await _context.GradeTypes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        if (item == null) return NotFound();

        item.IsActive = true;
        if (item.Name.EndsWith(" (nieaktywny)"))
            item.Name = item.Name.Replace(" (nieaktywny)", "").Trim();

        await _context.SaveChangesAsync();
        return Ok(new { message = "Przywrócono" });
    }
}

