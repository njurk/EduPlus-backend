using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BehaviorNoteTypeController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public BehaviorNoteTypeController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.BehaviorNoteTypes.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(BehaviorNoteType entity)
    {
        _context.BehaviorNoteTypes.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.BehaviorNoteTypes.FindAsync(id);
        if (item == null) return NotFound();
        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}