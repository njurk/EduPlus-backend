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
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
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
        var result = await _service.CreateAsync(entity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SchoolYear entity)
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
}