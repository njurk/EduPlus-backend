using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public ClassController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Classes.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Class entity)
    {
        _context.Classes.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Classes.FindAsync(id);
        if (item == null) return NotFound();
        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}