using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
            query = query.Where(x => x.OrderNumber.ToString().Contains(s)
                                  || x.StartTime.ToString().Contains(s)
                                  || x.EndTime.ToString().Contains(s));
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
    public async Task<IActionResult> Create([FromBody] LessonHour entity)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        entity.IsActive = true;

        _context.LessonHours.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LessonHour entity)
    {
        if (id != entity.Id) return BadRequest("ID elementu nie jest zgodne.");

        var existingItem = await _context.LessonHours.FindAsync(id);
        if (existingItem == null) return NotFound();

        existingItem.OrderNumber = entity.OrderNumber;
        existingItem.StartTime = entity.StartTime;
        existingItem.EndTime = entity.EndTime;
        existingItem.IsActive = entity.IsActive;
        existingItem.UpdatedAt = DateTime.Now;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Błąd zapisu danych");
        }

        return Ok(existingItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.LessonHours.FindAsync(id);
        if (item == null) return NotFound();

        _context.LessonHours.Remove(item);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Nie można usunąć tej godziny ponieważ jest ona używana w planie lekcji lub frekwencji");
        }

        return NoContent();
    }
}
