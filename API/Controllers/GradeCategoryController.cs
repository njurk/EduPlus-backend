using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeCategoryController : ControllerBase
{
    private readonly IGradeCategoryService _service;

    public GradeCategoryController(IGradeCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, string? sortBy, bool sortDesc = false, bool showInactive = false, bool includeSystem = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, showInactive, includeSystem));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeCategory entity)
    {
        try
        {
            var result = await _service.CreateAsync(entity);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeCategory entity)
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

    [HttpGet("semester/{order}")]
    public async Task<IActionResult> GetBySemester(int order)
    {
        var slug = order == 1 ? "midyear" : "final";
        var category = await _service.GetBySlugAsync(slug);
        if (category == null) return NotFound();
        return Ok(category);
    }
}