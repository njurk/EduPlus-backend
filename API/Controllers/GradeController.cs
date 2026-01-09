using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public GradeController(SchoolDbContext context) => _context = context;

    [HttpGet("current-semester/{schoolYearId}")]
    public async Task<IActionResult> GetCurrentSemester(int schoolYearId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var semesters = await _context.Semesters
            .AsNoTracking()
            .Where(s => s.SchoolYearId == schoolYearId)
            .OrderBy(s => s.StartDate)
            .ToListAsync();

        var current = semesters.FirstOrDefault(s => s.StartDate <= today && s.EndDate >= today);

        return Ok(current != null ? semesters.IndexOf(current) + 1 : 1);
    }

    [HttpGet("class-grades/{classId}/{subjectId}")]
    public async Task<IActionResult> GetClassGrades(int classId, int subjectId, [FromQuery] int semester = 1, [FromQuery] int? schoolYearId = null)
    {
        var yearId = schoolYearId ?? await _context.SchoolYears
            .Where(y => y.IsActive)
            .Select(y => y.Id)
            .FirstOrDefaultAsync();

        if (yearId == 0) return NotFound("Brak aktywnego roku");

        var semesters = await _context.Semesters
            .AsNoTracking()
            .Where(s => s.SchoolYearId == yearId)
            .OrderBy(s => s.StartDate)
            .ToListAsync();

        var targetSem = semesters.ElementAtOrDefault(semester - 1);
        if (targetSem == null) return BadRequest("Nieprawidłowy numer semestru");

        var start = targetSem.StartDate.ToDateTime(TimeOnly.MinValue);
        var end = targetSem.EndDate.ToDateTime(TimeOnly.MaxValue);

        var data = await _context.ClassStudents
            .AsNoTracking()
            .Where(cs => cs.ClassId == classId)
            .Select(cs => new
            {
                cs.StudentId,
                cs.Student.FirstName,
                cs.Student.LastName,
                cs.OrderNumber,
                Average = SchoolDbContext.CalculateWeightedAverage(cs.StudentId, subjectId, start, end),
                Grades = _context.Grades
                    .Where(g => g.StudentId == cs.StudentId &&
                                g.SubjectId == subjectId &&
                                g.IsActive &&
                                g.CreatedAt >= start &&
                                g.CreatedAt <= end)
                    .OrderBy(g => g.CreatedAt)
                    .Select(g => new
                    {
                        g.Id,
                        g.GradeTypeId,
                        g.GradeCategoryId,
                        g.Comment,
                        g.CreatedAt,
                        GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                        GradeCategory = new { g.GradeCategory.Name }
                    })
                    .ToList()
            })
            .OrderBy(x => x.OrderNumber)
            .ToListAsync();

        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Grade entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        if (entity.DateTime == default) entity.DateTime = DateTime.Now;
        _context.Grades.Add(entity);
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Grade entity)
    {
        var existing = await _context.Grades.FindAsync(id);
        if (existing == null) return NotFound();
        existing.GradeTypeId = entity.GradeTypeId;
        existing.GradeCategoryId = entity.GradeCategoryId;
        existing.Comment = entity.Comment;
        existing.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Grades.FindAsync(id);
        if (item == null) return NotFound();
        item.IsActive = false;
        item.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}