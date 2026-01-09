using API.DTOs;
using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassSubjectController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public ClassSubjectController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddSubject([FromBody] AddClassSubjectDto dto)
    {
        var exists = await _context.ClassSubjects
            .AnyAsync(cs => cs.ClassId == dto.ClassId && cs.SubjectId == dto.SubjectId);

        if (exists) return BadRequest("Przedmiot jest już przypisany do tej klasy");

        var cs = new ClassSubject
        {
            ClassId = dto.ClassId,
            SubjectId = dto.SubjectId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.ClassSubjects.Add(cs);
        await _context.SaveChangesAsync();
        return Ok(cs);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveSubject(int id)
    {
        var cs = await _context.ClassSubjects.FindAsync(id);
        if (cs == null) return NotFound();

        var teacherAssignment = await _context.TeacherClassSubjects
            .FirstOrDefaultAsync(tcs => tcs.ClassId == cs.ClassId && tcs.SubjectId == cs.SubjectId);

        if (teacherAssignment != null)
            _context.TeacherClassSubjects.Remove(teacherAssignment);

        _context.ClassSubjects.Remove(cs);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignTeacherAndSubject([FromBody] AssignTeacherSubjectDto dto)
    {
        var classSubject = await _context.ClassSubjects
            .FirstOrDefaultAsync(cs => cs.ClassId == dto.ClassId && cs.SubjectId == dto.SubjectId);

        if (classSubject == null)
        {
            classSubject = new ClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.ClassSubjects.Add(classSubject);
        }

        var teacherAssign = await _context.TeacherClassSubjects
            .FirstOrDefaultAsync(tcs => tcs.ClassId == dto.ClassId && tcs.SubjectId == dto.SubjectId);

        if (teacherAssign == null)
        {
            teacherAssign = new TeacherClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.TeacherClassSubjects.Add(teacherAssign);
        }
        else
        {
            teacherAssign.TeacherId = dto.TeacherId;
            teacherAssign.UpdatedAt = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Pomyślnie przypisano" });
    }
}
