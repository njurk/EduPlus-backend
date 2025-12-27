using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class LessonController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public LessonController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Lessons.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Lesson entity)
    {
        _context.Lessons.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Lessons.FindAsync(id);
        if (item == null) return NotFound();
        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}