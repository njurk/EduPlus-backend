using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var uid) ? (int?)uid : null;
        var success = await _service.DeleteAsync(id, userId);
        if (!success) return NotFound();
        return NoContent();
    }
}