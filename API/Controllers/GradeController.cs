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
    private readonly IEmailService _emailService;

    public GradeController(IGradeService service, IEmailService emailService)
    {
        _service = service;
        _emailService = emailService;
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
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] int? classId = null,
        [FromQuery] int? semesterId = null,
        [FromQuery] int? schoolYearId = null,
        [FromQuery] int? subjectId = null,
        [FromQuery] int? gradeTypeId = null,
        [FromQuery] int? gradeCategoryId = null,
        [FromQuery] int? teacherId = null,
        [FromQuery] bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(pageNumber, pageSize, search, sortBy, sortDesc, classId, semesterId, schoolYearId, subjectId, gradeTypeId, gradeCategoryId, teacherId, showInactive));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int teacherId)) return Unauthorized();

        try
        {
            var result = await _service.CreateAsync(dto, teacherId, _emailService);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
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

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var success = await _service.RestoreAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "Przywrócono", id });
    }
}