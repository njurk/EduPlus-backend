using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class LessonHourController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public LessonHourController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false, [FromQuery] bool showInactive = false)
    {
        var query = _context.LessonHours.AsNoTracking().AsQueryable();

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
            query = query.Where(x => x.OrderNumber.ToString().Contains(s) || x.StartTime.ToString("HH:mm").Contains(s) || x.EndTime.ToString("HH:mm").Contains(s));
        }

        query = sortBy?.ToLower() switch
        {
            "ordernumber" => sortDesc ? query.OrderByDescending(x => x.OrderNumber) : query.OrderBy(x => x.OrderNumber),
            "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
            _ => sortDesc ? query.OrderByDescending(x => x.OrderNumber) : query.OrderBy(x => x.OrderNumber)
        };

        return Ok(await query.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(LessonHour entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        entity.IsActive = true;

        _context.LessonHours.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LessonHour entity)
    {
        if (id != entity.Id) return BadRequest();

        entity.UpdatedAt = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.LessonHours.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.LessonHours.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;
        }
        else
        {
            _context.LessonHours.Remove(item);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}