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
    public async Task<IActionResult> GetAll(int? schoolYearId, bool includeInactive = true)
    {
        return Ok(await _service.GetAllAsync(schoolYearId, includeInactive));
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id, string sortBy = "lastName", bool sortDesc = false, string studentSearch = "", string subjectSearch = "", string subjectSortBy = "subjectName", bool subjectSortDesc = false)
    {
        var result = await _service.GetDetailsAsync(id, sortBy, sortDesc, studentSearch, subjectSearch, subjectSortBy, subjectSortDesc);
        if (result == null) return NotFound();
        return Ok(result);
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
}