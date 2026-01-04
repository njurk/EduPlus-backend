using Data.Data;
using Data.Data.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentStudentController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ParentStudentController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentStudentDto>>> GetAll()
        {
            var relations = await _context.ParentStudents
                .Include(ps => ps.Parent)
                .Include(ps => ps.Student)
                .Where(ps => ps.Parent.IsActive && ps.Student.IsActive)
                .Select(ps => new ParentStudentDto
                {
                    Id = ps.Id,
                    ParentId = ps.ParentId,
                    ParentName = $"{ps.Parent.FirstName} {ps.Parent.LastName}",
                    ParentEmail = ps.Parent.Email,
                    StudentId = ps.StudentId,
                    StudentName = $"{ps.Student.FirstName} {ps.Student.LastName}",
                    CreatedAt = ps.CreatedAt,
                    UpdatedAt = ps.UpdatedAt
                })
                .ToListAsync();

            return Ok(relations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParentStudentDto dto)
        {
            var exists = await _context.ParentStudents
                .AnyAsync(x => x.ParentId == dto.ParentId && x.StudentId == dto.StudentId);

            if (exists) return Conflict("To powiązanie już istnieje");

            var entity = new ParentStudent
            {
                ParentId = dto.ParentId,
                StudentId = dto.StudentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.ParentStudents.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(new { id = entity.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ParentStudents.FindAsync(id);
            if (item == null) return NotFound();

            _context.ParentStudents.Remove(item);

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}