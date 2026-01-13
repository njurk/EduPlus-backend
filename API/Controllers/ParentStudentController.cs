using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParentStudentController : ControllerBase
    {
        private readonly EduPlusDbContext _context;

        public ParentStudentController(EduPlusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentStudentDto>>> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false)
        {
            var query = _context.ParentStudents
                .Include(ps => ps.Parent)
                .Include(ps => ps.Student)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(ps =>
                    ps.Parent.LastName.Contains(s) ||
                    ps.Parent.FirstName.Contains(s) ||
                    ps.Student.LastName.Contains(s) ||
                    ps.Student.FirstName.Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "parentname" => sortDesc ? query.OrderByDescending(ps => ps.Parent.LastName).ThenByDescending(ps => ps.Parent.FirstName)
                                         : query.OrderBy(ps => ps.Parent.LastName).ThenBy(ps => ps.Parent.FirstName),
                "studentname" => sortDesc ? query.OrderByDescending(ps => ps.Student.LastName).ThenByDescending(ps => ps.Student.FirstName)
                                          : query.OrderBy(ps => ps.Student.LastName).ThenBy(ps => ps.Student.FirstName),
                "created" => sortDesc ? query.OrderByDescending(ps => ps.CreatedAt) : query.OrderBy(ps => ps.CreatedAt),
                _ => sortDesc ? query.OrderByDescending(ps => ps.CreatedAt) : query.OrderBy(ps => ps.CreatedAt)
            };

            var relations = await query
                .Select(ps => new ParentStudentDto
                {
                    Id = ps.Id,
                    ParentId = ps.ParentId,
                    ParentName = $"{ps.Parent.LastName} {ps.Parent.FirstName}",
                    ParentEmail = ps.Parent.Email,
                    StudentId = ps.StudentId,
                    StudentName = $"{ps.Student.LastName} {ps.Student.FirstName}",
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

            if (exists) return Conflict("To powi¹zanie ju¿ istnieje");

            var entity = new ParentStudent
            {
                ParentId = dto.ParentId,
                StudentId = dto.StudentId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
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
