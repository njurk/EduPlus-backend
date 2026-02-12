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
    private readonly IEmailService _emailService;

    public AttendanceController(IAttendanceService service, IEmailService emailService)
    {
        _service = service;
        _emailService = emailService;
    }

    [HttpGet("admin")]
    public async Task<IActionResult> GetAllForAdmin(
        [FromQuery] bool showInactive = false,
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
        [FromQuery] int? orderNumber = null,
        [FromQuery] int? semesterId = null)
    {
        var result = await _service.GetAllForAdminAsync(showInactive, pageNumber, pageSize, search, sortBy, sortDesc, classId, date, subjectName, teacherName, attendanceTypeShortCode, orderNumber, semesterId);
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
        try
        {
            var result = await _service.UpdateAsync(id, dto.AttendanceTypeId, _emailService);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
