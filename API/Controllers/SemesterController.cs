using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SemesterController : ControllerBase
{
    private readonly ISemesterService _service;

    public SemesterController(ISemesterService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int? schoolYearId = null)
    {
        return Ok(await _service.GetAllAsync(schoolYearId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(Semester entity)
    {
        var result = await _service.CreateAsync(entity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Semester entity)
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