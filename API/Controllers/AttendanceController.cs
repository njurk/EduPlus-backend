using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service)
    {
        _service = service;
    }

    [HttpGet("admin")]
    public async Task<IActionResult> GetAllForAdmin(
        [FromQuery] bool includeInactive = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] int? classId = null,
        [FromQuery] DateTime? date = null,
        [FromQuery] string? subjectName = null,
        [FromQuery] string? teacherName = null,
        [FromQuery] string? attendanceTypeShortCode = null,
        [FromQuery] int? orderNumber = null)
    {
        var result = await _service.GetAllForAdminAsync(includeInactive, pageNumber, pageSize, search, sortBy, sortDesc, classId, date, subjectName, teacherName, attendanceTypeShortCode, orderNumber);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
    {
        var result = await _service.UpdateAsync(id, dto.AttendanceTypeId);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
