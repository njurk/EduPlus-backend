using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SemesterController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public SemesterController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? schoolYearId)
    {
        var query = _context.Semesters.AsQueryable();
        if (schoolYearId.HasValue) query = query
                .Where(s => s.SchoolYearId == schoolYearId);

        return Ok(await query.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Semester semester)
    {
        semester.CreatedAt = DateTime.Now;
        semester.UpdatedAt = DateTime.Now;
        _context.Semesters.Add(semester);
        await _context.SaveChangesAsync();
        return Ok(semester);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Semester semester)
    {
        if (id != semester.Id) return BadRequest();
        var dbSem = await _context.Semesters.FindAsync(id);
        if (dbSem == null) return NotFound();

        dbSem.Name = semester.Name;
        dbSem.StartDate = semester.StartDate;
        dbSem.EndDate = semester.EndDate;
        dbSem.IsActive = semester.IsActive;
        dbSem.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(dbSem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var semester = await _context.Semesters.FindAsync(id);
        if (semester == null) return NotFound();

        if (semester.IsActive) { semester.IsActive = false; semester.UpdatedAt = DateTime.Now; }
        else { _context.Semesters.Remove(semester); }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
