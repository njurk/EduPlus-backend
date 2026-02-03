using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectController(ISubjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, showInactive));
    }

    [HttpGet("{id}/teachers")]
    public async Task<IActionResult> GetTeachers(int id)
    {
        return Ok(await _service.GetTeachersAsync(id));
    }

    [HttpGet("teachers")]
    public async Task<IActionResult> GetAllSubjectTeachers(int pageNumber = 1, int pageSize = 20, string? sortBy = null, bool sortDesc = false, string? search = null, int? subjectId = null, int? teacherId = null, bool showInactive = false)
    {
        return Ok(await _service.GetAllSubjectTeachersAsync(pageNumber, pageSize, sortBy, sortDesc, search, subjectId, teacherId, showInactive));
    }

    [HttpPost("{subjectId}/teachers/{teacherId}")]
    public async Task<IActionResult> AddTeacher(int subjectId, int teacherId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.AddTeacherToSubjectAsync(subjectId, teacherId, userId);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{subjectId}/teachers/{teacherId}")]
    public async Task<IActionResult> RemoveTeacher(int subjectId, int teacherId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.RemoveTeacherFromSubjectAsync(subjectId, teacherId, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost("{subjectId}/teachers/{teacherId}/restore")]
    public async Task<IActionResult> RestoreTeacher(int subjectId, int teacherId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.RestoreTeacherSubjectAsync(subjectId, teacherId, userId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{subjectId}/teachers/{oldTeacherId}")]
    public async Task<IActionResult> UpdateTeacher(int subjectId, int oldTeacherId, [FromBody] UpdateSubjectTeacherDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _service.UpdateSubjectTeacherAsync(subjectId, oldTeacherId, dto.NewTeacherId, userId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Subject entity)
    {
        try
        {
            var result = await _service.CreateAsync(entity);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Subject entity)
    {
        try
        {
            var result = await _service.UpdateAsync(id, entity);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var success = await _service.RestoreAsync(id);
        if (!success) return NotFound();
        return Ok();
    }
}