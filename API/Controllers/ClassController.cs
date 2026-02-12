using BusinessLogic.Services;
using Data.Data.Entities;
using Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassController : ControllerBase
{
    private readonly IClassService _service;

    public ClassController(IClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int? schoolYearId, bool showInactive = false, int pageNumber = 1, int pageSize = 20, string? sortBy = null, bool sortDesc = false, int? level = null, string? search = null)
    {
        return Ok(await _service.GetAllAsync(schoolYearId, showInactive, pageNumber, pageSize, sortBy, sortDesc, level, search));
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id, string sortBy = "lastName", bool sortDesc = false, string studentSearch = "", string subjectSearch = "", string subjectSortBy = "subjectName", bool subjectSortDesc = false, bool showInactiveSubjects = false, bool showInactiveStudents = false)
    {
        var result = await _service.GetDetailsAsync(id, sortBy, sortDesc, studentSearch, subjectSearch, subjectSortBy, subjectSortDesc, showInactiveSubjects, showInactiveStudents);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}/student-parents")]
    public async Task<IActionResult> GetStudentsWithParents(int id)
    {
        return Ok(await _service.GetStudentsWithParentsAsync(id));
    }

    [HttpGet("{classId}/candidates")]
    public async Task<IActionResult> GetStudentCandidates(int classId, string search = "")
    {
        return Ok(await _service.GetCandidatesAsync(classId, search));
    }

    [HttpPost("students/bulk")]
    public async Task<IActionResult> AddStudentsBulk([FromBody] BulkAddStudentsDto dto)
    {
        await _service.AddStudentsBulkAsync(dto.ClassId, dto.StudentIds);
        return Ok();
    }

    [HttpPost("subjects/assign")]
    public async Task<IActionResult> AssignSubject([FromBody] AssignSubjectDto dto)
    {
        try
        {
            await _service.AssignSubjectAsync(dto.ClassId, dto.SubjectId, dto.TeacherId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("subjects/{classSubjectId}")]
    public async Task<IActionResult> UpdateSubjectTeacher(int classSubjectId, [FromBody] UpdateSubjectTeacherDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.UpdateSubjectTeacherAsync(classSubjectId, dto.TeacherId, userId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Class entity)
    {
        try
        {
            var result = await _service.CreateAsync(entity);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Class entity)
    {
        try
        {
            var result = await _service.UpdateAsync(id, entity);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return BadRequest("Nieprawidłowe ID");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("students/{classStudentId}")]
    public async Task<IActionResult> RemoveStudent(int classStudentId)
    {
        try
        {
            await _service.RemoveStudentAsync(classStudentId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("students/{classStudentId}/restore")]
    public async Task<IActionResult> RestoreStudent(int classStudentId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.RestoreStudentAsync(classStudentId, userId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("subjects/{classSubjectId}")]
    public async Task<IActionResult> RemoveSubject(int classSubjectId)
    {
        try
        {
            await _service.RemoveSubjectAsync(classSubjectId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("subjects/{classSubjectId}/restore")]
    public async Task<IActionResult> RestoreSubject(int classSubjectId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.RestoreSubjectAsync(classSubjectId, userId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
