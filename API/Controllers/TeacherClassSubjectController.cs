using API.DTOs;
using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherClassSubjectController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public TeacherClassSubjectController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Assign([FromBody] AssignTeacherDto dto)
    {
        var existing = await _context.TeacherClassSubjects
            .FirstOrDefaultAsync(t => t.ClassId == dto.ClassId && t.SubjectId == dto.SubjectId);

        if (existing != null)
        {
            existing.TeacherId = dto.TeacherId;
            existing.UpdatedAt = DateTime.Now;
        }
        else
        {
            _context.TeacherClassSubjects.Add(new TeacherClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Unassign([FromQuery] int classId, [FromQuery] int subjectId)
    {
        var assignment = await _context.TeacherClassSubjects
            .FirstOrDefaultAsync(t => t.ClassId == classId && t.SubjectId == subjectId);

        if (assignment == null) return NotFound();

        _context.TeacherClassSubjects.Remove(assignment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
