using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public ClassController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? schoolYearId, [FromQuery] bool includeInactive = true)
    {
        var query = _context.Classes.AsNoTracking();

        if (schoolYearId.HasValue)
            query = query.Where(c => c.SchoolYearId == schoolYearId);

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        var classes = await query
            .Include(c => c.ClassStudents)
            .Select(c => new
            {
                c.Id,
                c.Level,
                c.Letter,
                c.SchoolYearId,
                c.IsActive,
                StudentCount = c.ClassStudents.Count
            })
            .OrderBy(c => c.Level).ThenBy(c => c.Letter)
            .ToListAsync();

        return Ok(classes);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(
        int id,
        [FromQuery] string sortBy = "lastName",
        [FromQuery] bool sortDesc = false,
        [FromQuery] string studentSearch = "",
        [FromQuery] string subjectSearch = "",
        [FromQuery] string subjectSortBy = "subjectName",
        [FromQuery] bool subjectSortDesc = false
        )
    {
        var classEntity = await _context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (classEntity == null) return NotFound();

        var studentsQuery = _context.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == id)
            .Include(cs => cs.Student)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(studentSearch))
        {
            var s = studentSearch.Trim().ToLower();
            studentsQuery = studentsQuery.Where(cs =>
                cs.Student.LastName.ToLower().Contains(s) ||
                cs.Student.FirstName.ToLower().Contains(s) ||
                cs.Student.Email.ToLower().Contains(s));
        }

        studentsQuery = sortBy.ToLower() switch
        {
            "id" => sortDesc ? studentsQuery.OrderByDescending(x => x.OrderNumber).ThenByDescending(x => x.Student.LastName) : studentsQuery.OrderBy(x => x.OrderNumber).ThenBy(x => x.Student.LastName),
            "email" => sortDesc ? studentsQuery.OrderByDescending(x => x.Student.Email) : studentsQuery.OrderBy(x => x.Student.Email),
            "createdat" => sortDesc ? studentsQuery.OrderByDescending(x => x.CreatedAt) : studentsQuery.OrderBy(x => x.CreatedAt),
            "updatedat" => sortDesc ? studentsQuery.OrderByDescending(x => x.UpdatedAt) : studentsQuery.OrderBy(x => x.UpdatedAt),
            _ => sortDesc ? studentsQuery.OrderByDescending(x => x.Student.LastName).ThenByDescending(x => x.Student.FirstName) : studentsQuery.OrderBy(x => x.Student.LastName).ThenBy(x => x.Student.FirstName),
        };

        var students = await studentsQuery.Select(cs => new
        {
            cs.Id,
            cs.StudentId,
            cs.OrderNumber,
            cs.CreatedAt,
            cs.UpdatedAt,
            Student = new { cs.Student.Id, cs.Student.FirstName, cs.Student.LastName, cs.Student.Email }
        }).ToListAsync();

        var subjectsQuery = _context.ClassSubjects.AsNoTracking()
            .Where(cs => cs.ClassId == id)
            .Include(cs => cs.Subject)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(subjectSearch))
        {
            var s = subjectSearch.Trim().ToLower();
            subjectsQuery = subjectsQuery.Where(cs => cs.Subject.Name.ToLower().Contains(s));
        }

        var rawSubjects = await subjectsQuery.Select(cs => new
        {
            cs.Id,
            cs.ClassId,
            cs.SubjectId,
            SubjectName = cs.Subject.Name,
            cs.CreatedAt,
            cs.UpdatedAt,
            TeacherInfo = _context.TeacherClassSubjects
                .Where(t => t.ClassId == id && t.SubjectId == cs.SubjectId)
                .Select(t => new { t.TeacherId, Name = t.Teacher.LastName + " " + t.Teacher.FirstName })
                .FirstOrDefault()
        }).ToListAsync();

        var sortedSubjects = subjectSortBy.ToLower() switch
        {
            "teachername" => subjectSortDesc
                ? rawSubjects.OrderByDescending(x => x.TeacherInfo?.Name).ToList()
                : rawSubjects.OrderBy(x => x.TeacherInfo?.Name).ToList(),
            "createdat" => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.CreatedAt).ToList() : rawSubjects.OrderBy(x => x.CreatedAt).ToList(),
            "updatedat" => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.UpdatedAt).ToList() : rawSubjects.OrderBy(x => x.UpdatedAt).ToList(),
            _ => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.SubjectName).ToList() : rawSubjects.OrderBy(x => x.SubjectName).ToList()
        };

        var subjects = sortedSubjects.Select(s => new
        {
            s.Id,
            s.ClassId,
            s.SubjectId,
            s.SubjectName,
            s.CreatedAt,
            s.UpdatedAt,
            TeacherId = s.TeacherInfo?.TeacherId,
            TeacherName = s.TeacherInfo?.Name
        });

        return Ok(new
        {
            ClassInfo = classEntity,
            Students = students,
            Subjects = subjects
        });
    }

    [HttpGet("{classId}/candidates")]
    public async Task<IActionResult> GetStudentCandidates(int classId, [FromQuery] string search = "")
    {
        var query = _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive && u.UserRoles.Any(ur => ur.Role.Name == "Uczeń"));

        query = query.Where(u => !u.ClassStudents.Any(cs => cs.ClassId == classId));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(u =>
                u.LastName.ToLower().Contains(s) ||
                u.FirstName.ToLower().Contains(s) ||
                u.Email.ToLower().Contains(s));
        }

        var candidates = await query
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(50)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email
            })
            .ToListAsync();

        return Ok(candidates);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Class classEntity)
    {
        var exists = await _context.Classes.AnyAsync(c =>
            c.SchoolYearId == classEntity.SchoolYearId &&
            c.Level == classEntity.Level &&
            c.Letter == classEntity.Letter
        );

        if (exists)
        {
            return BadRequest($"Klasa {classEntity.Level}{classEntity.Letter} już istnieje w wybranym roku szkolnym");
        }

        classEntity.CreatedAt = DateTime.UtcNow;
        classEntity.UpdatedAt = DateTime.UtcNow;
        classEntity.IsActive = true;
        _context.Classes.Add(classEntity);
        await _context.SaveChangesAsync();
        return Ok(classEntity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Class classEntity)
    {
        if (id != classEntity.Id) return BadRequest();

        var exists = await _context.Classes.AnyAsync(c =>
            c.SchoolYearId == classEntity.SchoolYearId &&
            c.Level == classEntity.Level &&
            c.Letter == classEntity.Letter &&
            c.Id != id
        );

        if (exists)
        {
            return BadRequest($"Klasa {classEntity.Level}{classEntity.Letter} już istnieje w tym roku szkolnym");
        }

        var dbClass = await _context.Classes.FindAsync(id);
        if (dbClass == null) return NotFound();

        dbClass.Level = classEntity.Level;
        dbClass.Letter = classEntity.Letter;
        dbClass.IsActive = classEntity.IsActive;
        dbClass.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(dbClass);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var classEntity = await _context.Classes.FindAsync(id);
        if (classEntity == null) return NotFound();

        if (classEntity.IsActive)
        {
            classEntity.IsActive = false;
            classEntity.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _context.Classes.Remove(classEntity);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}