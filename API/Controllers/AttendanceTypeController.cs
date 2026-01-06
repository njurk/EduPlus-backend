using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttendanceTypeController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public AttendanceTypeController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.AttendanceTypes
            .AsNoTracking()
            .OrderBy(x => x.ShortCode)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AttendanceType entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        entity.IsActive = true;

        _context.AttendanceTypes.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AttendanceType entity)
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
            if (!await _context.AttendanceTypes.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.AttendanceTypes.FindAsync(id);
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
            _context.AttendanceTypes.Remove(item);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}