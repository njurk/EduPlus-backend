using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SchoolYearController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public SchoolYearController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.SchoolYears.OrderByDescending(y => y.StartDate).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var year = await _context.SchoolYears
            .Include(y => y.Semesters)
            .FirstOrDefaultAsync(y => y.Id == id);

        if (year == null) return NotFound();
        return Ok(year);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SchoolYear year)
    {
        year.CreatedAt = DateTime.Now;
        year.UpdatedAt = DateTime.Now;
        _context.SchoolYears.Add(year);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = year.Id }, year);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SchoolYear year)
    {
        if (id != year.Id) return BadRequest();
        var dbYear = await _context.SchoolYears.FindAsync(id);
        if (dbYear == null) return NotFound();

        dbYear.Name = year.Name;
        dbYear.StartDate = year.StartDate;
        dbYear.EndDate = year.EndDate;
        dbYear.IsActive = year.IsActive;
        dbYear.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(dbYear);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var year = await _context.SchoolYears.FindAsync(id);
        if (year == null) return NotFound();

        if (year.IsActive) { year.IsActive = false; year.UpdatedAt = DateTime.Now; }
        else { _context.SchoolYears.Remove(year); }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}