using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public AttendanceController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Attendances.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Attendance entity)
    {
        _context.Attendances.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Attendances.FindAsync(id);
        if (item == null) return NotFound();
        item.IsActive = false;
        item.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}