using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonController : ControllerBase
{
    private readonly ILessonService _service;

    public LessonController(ILessonService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] int? classId = null,
        [FromQuery] int? subjectId = null,
        [FromQuery] int? semesterId = null,
        [FromQuery] int? schoolYearId = null,
        [FromQuery] bool showInactive = false)
    {
        var result = await _service.GetAllAsync(search, sortBy, sortDesc, classId, subjectId, semesterId, schoolYearId, showInactive);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var item = await _service.GetDetailsAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("{id}/attendance")]
    public async Task<IActionResult> GetAttendance(int id)
    {
        var result = await _service.GetLessonAttendanceAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id}/attendance/{studentId}")]
    public async Task<IActionResult> UpdateAttendance(int id, int studentId, [FromBody] UpdateLessonAttendanceDto dto)
    {
        var success = await _service.UpdateLessonAttendanceAsync(id, studentId, dto.AttendanceTypeId);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
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

    [HttpPost("from-schedule")]
    public async Task<IActionResult> CreateFromSchedule([FromBody] CreateFromScheduleDto dto)
    {
        try
        {
            var result = await _service.CreateFromScheduleAsync(dto.ScheduleId, dto.Date, dto.TeacherId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}

