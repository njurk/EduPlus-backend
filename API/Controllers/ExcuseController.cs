using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExcuseController : ControllerBase
{
    private readonly IExcuseService _service;

    public ExcuseController(IExcuseService service)
    {
        _service = service;
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] bool showInactive = false,
        [FromQuery] string? statusFilter = null,
        [FromQuery] int? classId = null)
    {
        return Ok(await _service.GetAllAsync(pageNumber, pageSize, search, sortBy, sortDesc, showInactive, statusFilter, classId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExcuseDto dto)
    {
        var result = await _service.CreateAsync(dto, GetUserId());
        return Ok(result);
    }

    [HttpPatch("{id}/accept")]
    public async Task<IActionResult> Accept(int id, [FromBody] AcceptExcuseDto dto)
    {
        var success = await _service.AcceptAsync(id, dto.IsAccepted, GetUserId());
        return success ? NoContent() : NotFound();
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var success = await _service.RestoreAsync(id, GetUserId());
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id, GetUserId());
        return success ? NoContent() : NotFound();
    }
}
