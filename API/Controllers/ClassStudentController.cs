using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassStudentController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public ClassStudentController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.ClassStudents.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> Create(ClassStudent entity)
    {
        _context.ClassStudents.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ClassStudents.FindAsync(id);
        if (item == null) return NotFound();
        item.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}