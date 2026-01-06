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
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.LessonHours
            .AsNoTracking()
            .OrderBy(x => x.OrderNumber)
            .ToListAsync();
        return Ok(items);
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