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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AttendanceType entity)
    {
        var result = await _service.CreateAsync(entity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AttendanceType entity)
    {
        if (id != entity.Id) return BadRequest("ID mismatch");

        var result = await _service.UpdateAsync(id, entity);
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