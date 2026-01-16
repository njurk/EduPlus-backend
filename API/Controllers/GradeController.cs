using Shared.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeController : ControllerBase
{
    private readonly IGradeService _service;

    public GradeController(IGradeService service)
    {
        _service = service;
    }

    [HttpGet("current-semester/{schoolYearId}")]
    public async Task<IActionResult> GetCurrentSemester(int schoolYearId)
    {
        return Ok(await _service.GetCurrentSemesterAsync(schoolYearId));
    }

    [HttpGet("class-grades/{classId}/{subjectId}")]
    public async Task<IActionResult> GetClassGrades(int classId, int subjectId, [FromQuery] int semester = 1, [FromQuery] int? schoolYearId = null)
    {
        try
        {
            var result = await _service.GetClassGradesAsync(classId, subjectId, semester, schoolYearId);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] int? classId = null,
        [FromQuery] bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, classId, showInactive));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int teacherId)) return Unauthorized();

        var result = await _service.CreateAsync(dto, teacherId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}