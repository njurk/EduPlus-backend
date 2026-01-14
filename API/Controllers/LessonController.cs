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
        [FromQuery] int? subjectId = null)
    {
        var result = await _service.GetAllAsync(search, sortBy, sortDesc, classId, subjectId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
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

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateLessons([FromBody] GenerateLessonsDto dto)
    {
        var count = await _service.GenerateLessonsAsync(dto.ClassId, dto.SchoolYearId, dto.SemesterId);
        return Ok(new { GeneratedCount = count });
    }
}

