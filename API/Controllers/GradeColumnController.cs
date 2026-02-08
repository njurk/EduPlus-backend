using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeColumnController : ControllerBase
{
    private readonly IGradeColumnService _service;

    public GradeColumnController(IGradeColumnService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int classId, [FromQuery] int subjectId, [FromQuery] int semesterId)
    {
        return Ok(await _service.GetAllAsync(classId, subjectId, semesterId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GradeColumnCreateDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int teacherId)) return Unauthorized();

        var result = await _service.CreateAsync(dto.ClassId, dto.SubjectId, dto.SemesterId, dto.GradeCategoryId, teacherId, dto.Name);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] GradeColumnUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto.Name, dto.GradeCategoryId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
