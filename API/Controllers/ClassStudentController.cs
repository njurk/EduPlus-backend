using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ClassStudentController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public ClassStudentController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent([FromBody] AddStudentDto dto)
    {
        var exists = await _context.ClassStudents
            .AnyAsync(cs => cs.ClassId == dto.ClassId && cs.StudentId == dto.StudentId);

        if (exists) return BadRequest("Uczeń jest już przypisany do tej klasy");

        var cs = new ClassStudent
        {
            ClassId = dto.ClassId,
            StudentId = dto.StudentId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.ClassStudents.Add(cs);
        await _context.SaveChangesAsync();
        return Ok(cs);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveStudent(int id)
    {
        var cs = await _context.ClassStudents.FindAsync(id);
        if (cs == null) return NotFound();
        _context.ClassStudents.Remove(cs);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}