using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementReadController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public AnnouncementReadController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.AnnouncementReads.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(AnnouncementRead entity)
    {
        _context.AnnouncementReads.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.AnnouncementReads.FindAsync(id);
        if (item == null) return NotFound();
        item.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}