using Shared.DTOs;
using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeController : ControllerBase
{
    private readonly EduPlusDbContext _context;

    public GradeController(EduPlusDbContext context) => _context = context;

    [HttpGet("current-semester/{schoolYearId}")]
    public async Task<IActionResult> GetCurrentSemester(int schoolYearId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var semesters = await _context.Semesters.AsNoTracking()
            .Where(s => s.SchoolYearId == schoolYearId).OrderBy(s => s.StartDate).ToListAsync();
        var current = semesters.FirstOrDefault(s => s.StartDate <= today && s.EndDate >= today);
        return Ok(current != null ? semesters.IndexOf(current) + 1 : 1);
    }

    [HttpGet("class-grades/{classId}/{subjectId}")]
    public async Task<IActionResult> GetClassGrades(int classId, int subjectId, [FromQuery] int semester = 1, [FromQuery] int? schoolYearId = null)
    {
        var yearId = schoolYearId ?? await _context.SchoolYears.Where(y => y.IsActive).Select(y => y.Id).FirstOrDefaultAsync();
        if (yearId == 0) return NotFound("Brak aktywnego roku");

        var semesters = await _context.Semesters.AsNoTracking()
            .Where(s => s.SchoolYearId == yearId).OrderBy(s => s.StartDate).ToListAsync();
        var targetSem = semesters.ElementAtOrDefault(semester - 1);
        if (targetSem == null) return BadRequest("Nieprawid³owy numer semestru");

        var start = targetSem.StartDate.ToDateTime(TimeOnly.MinValue);
        var end = targetSem.EndDate.ToDateTime(TimeOnly.MaxValue);

        var data = await _context.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == classId)
            .Select(cs => new
            {
                cs.Id,
                cs.StudentId,
                cs.Student.FirstName,
                cs.Student.LastName,
                cs.OrderNumber,
                Average = EduPlusDbContext.CalculateWeightedAverage(cs.StudentId, subjectId, start, end),
                Grades = _context.Grades
                    .Where(g => g.StudentId == cs.StudentId && g.SubjectId == subjectId && g.IsActive && g.CreatedAt >= start && g.CreatedAt <= end)
                    .OrderBy(g => g.CreatedAt)
                    .Select(g => new
                    {
                        g.Id,
                        g.GradeTypeId,
                        g.GradeCategoryId,
                        g.Comment,
                        g.CreatedAt,
                        GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                        GradeCategory = new { g.GradeCategory.Name },
                        TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                    }).ToList()
            })
            .OrderBy(x => x.OrderNumber).ToListAsync();

        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int teacherId)) return Unauthorized();

        var entity = new Grade
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            GradeTypeId = dto.GradeTypeId,
            GradeCategoryId = dto.GradeCategoryId,
            Comment = dto.Comment,
            TeacherId = teacherId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsActive = true
        };

        _context.Grades.Add(entity);
        await _context.SaveChangesAsync();

        var createdGrade = await _context.Grades
            .Include(g => g.GradeType)
            .Include(g => g.GradeCategory)
            .Include(g => g.Teacher)
            .Select(g => new
            {
                g.Id,
                g.GradeTypeId,
                g.GradeCategoryId,
                g.Comment,
                g.CreatedAt,
                GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                GradeCategory = new { g.GradeCategory.Name },
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
            })
            .FirstOrDefaultAsync(g => g.Id == entity.Id);

        return Ok(createdGrade);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeDto dto)
    {
        var existing = await _context.Grades
            .Include(g => g.GradeType)
            .Include(g => g.GradeCategory)
            .Include(g => g.Teacher)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (existing == null) return NotFound();

        existing.GradeTypeId = dto.GradeTypeId;
        existing.GradeCategoryId = dto.GradeCategoryId;
        existing.Comment = dto.Comment;
        existing.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        if (dto.GradeTypeId != existing.GradeTypeId) await _context.Entry(existing).Reference(g => g.GradeType).LoadAsync();
        if (dto.GradeCategoryId != existing.GradeCategoryId) await _context.Entry(existing).Reference(g => g.GradeCategory).LoadAsync();

        return Ok(new
        {
            existing.Id,
            existing.GradeTypeId,
            existing.GradeCategoryId,
            existing.Comment,
            existing.CreatedAt,
            GradeType = new { existing.GradeType.Numeric, existing.GradeType.Name, existing.GradeType.Value },
            GradeCategory = new { existing.GradeCategory.Name },
            TeacherName = existing.Teacher.FirstName + " " + existing.Teacher.LastName
        });
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
