using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GradeTypeController : ControllerBase
{
    private readonly IGradeTypeService _service;

    public GradeTypeController(IGradeTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, string? sortBy, bool sortDesc = false, bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, showInactive));
    }

    [HttpPost]
    public async Task<IActionResult> Create(GradeType entity)
    {
        var result = await _service.CreateAsync(entity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, GradeType entity)
    {
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

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var success = await _service.RestoreAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "Przywrocono", id });
    }
}