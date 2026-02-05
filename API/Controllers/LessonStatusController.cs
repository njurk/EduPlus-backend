using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonStatusController : ControllerBase
{
    private readonly ILessonStatusService _service;

    public LessonStatusController(ILessonStatusService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, string? sortBy, bool sortDesc = false, bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, showInactive));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LessonStatus entity)
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
}