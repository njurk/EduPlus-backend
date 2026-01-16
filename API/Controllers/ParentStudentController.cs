using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParentStudentController : ControllerBase
{
    private readonly IParentStudentService _service;

    public ParentStudentController(IParentStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search = null, string? sortBy = null, bool sortDesc = false)
    {
        return Ok(await _service.GetAllAsync(search, sortBy, sortDesc));
    }

    [HttpPost]
    public async Task<IActionResult> Create(ParentStudent entity)
    {
        var result = await _service.CreateAsync(entity);
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