using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchoolYearController : ControllerBase
{
    private readonly ISchoolYearService _service;

    public SchoolYearController(ISchoolYearService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc, showInactive));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}/semesters")]
    public async Task<IActionResult> GetSemesters(int id)
    {
        return Ok(await _service.GetSemestersAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create(SchoolYear entity)
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
    public async Task<IActionResult> Update(int id, SchoolYear entity)
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
        return Ok();
    }
}