using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public SubjectController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false, [FromQuery] bool showInactive = false)
    {
        var query = _context.Subjects.AsNoTracking().AsQueryable();

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
            query = query.Where(x => x.Name.Contains(s));
        }

        query = sortBy?.ToLower() switch
        {
            "ordernumber" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
            _ => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
        };

        return Ok(await query.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Subject entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        entity.IsActive = true;

        _context.Subjects.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Subject entity)
    {
        if (id != entity.Id) return BadRequest();

        if (entity.IsActive && entity.Name.EndsWith(" (nieaktywny)"))
        {
            entity.Name = entity.Name.Replace(" (nieaktywny)", "");
        }

        entity.UpdatedAt = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Subjects.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Subjects.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            item.IsActive = false;
            if (!item.Name.EndsWith(" (nieaktywny)"))
            {
                item.Name += " (nieaktywny)";
            }
            item.UpdatedAt = DateTime.Now;
        }
        else
        {
            _context.Subjects.Remove(item);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
