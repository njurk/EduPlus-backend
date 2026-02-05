using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceTypeController : ControllerBase
{
    private readonly IAttendanceTypeService _service;

    public AttendanceTypeController(IAttendanceTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? sortBy, [FromQuery] bool sortDesc = false, [FromQuery] bool showInactive = false)
    {
        var result = await _service.GetAllAsync(search, sortBy, sortDesc, showInactive);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AttendanceType entity)
    {
        try
        {
            if (id != entity.Id) return BadRequest("Złe ID");

            var result = await _service.UpdateAsync(id, entity);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
