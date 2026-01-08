using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public GradeController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Grades.ToListAsync());

    [HttpGet("class-grades/{classId}/{subjectId}")]
    public async Task<IActionResult> GetClassGrades(
        int classId,
        int subjectId,
        [FromQuery] int semester = 1,
        [FromQuery] int? schoolYearId = null,
        [FromQuery] string sortBy = "orderNumber",
        [FromQuery] bool sortDesc = false
    )
    {
        var yearQuery = _context.SchoolYears
            .Include(y => y.Semesters)
            .AsNoTracking();

        var year = schoolYearId.HasValue
            ? await yearQuery.FirstOrDefaultAsync(y => y.Id == schoolYearId)
            : await yearQuery.FirstOrDefaultAsync(y => y.IsActive);

        if (year == null) return NotFound("Nie znaleziono roku szkolnego");

        var targetSemester = year.Semesters
            .OrderBy(s => s.StartDate)
            .ElementAtOrDefault(semester - 1);

        if (targetSemester == null) return BadRequest("Nie znaleziono wybranego semestru dla tego roku");

        var semStart = targetSemester.StartDate.ToDateTime(TimeOnly.MinValue);
        var semEnd = targetSemester.EndDate.ToDateTime(TimeOnly.MaxValue);

        var query = _context.ClassStudents
            .AsNoTracking()
            .Where(cs => cs.ClassId == classId);

        var rawData = query.Select(cs => new
        {
            cs.StudentId,
            cs.Student.FirstName,
            cs.Student.LastName,
            cs.Student.Email,
            cs.OrderNumber,
            Average = SchoolDbContext.CalculateWeightedAverage(cs.StudentId, subjectId, semStart, semEnd),
            Grades = _context.Grades
                .Where(g => g.StudentId == cs.StudentId &&
                            g.SubjectId == subjectId &&
                            g.IsActive &&
                            g.CreatedAt >= semStart &&
                            g.CreatedAt <= semEnd)
                .OrderBy(g => g.CreatedAt)
                .Select(g => new
                {
                    g.Id,
                    g.StudentId,
                    g.SubjectId,
                    g.GradeTypeId,
                    g.GradeCategoryId,
                    g.Comment,
                    g.CreatedAt,
                    GradeType = new
                    {
                        g.GradeType.Id,
                        g.GradeType.Numeric,
                        g.GradeType.Name,
                        g.GradeType.Value
                    },
                    GradeCategory = new
                    {
                        g.GradeCategory.Id,
                        g.GradeCategory.Name
                    }
                })
                .ToList()
        });

        if (sortBy.ToLower() == "average")
        {
            rawData = sortDesc ? rawData.OrderByDescending(x => x.Average) : rawData.OrderBy(x => x.Average);
        }
        else
        {
            rawData = sortDesc ? rawData.OrderByDescending(x => x.OrderNumber) : rawData.OrderBy(x => x.OrderNumber);
        }

        return Ok(await rawData.ToListAsync());
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