using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

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

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] bool? isAccepted = null)
    {
        var result = await _service.GetAllAsync(search, sortBy, sortDesc, isAccepted);
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
    public async Task<IActionResult> Create([FromBody] CreateExcuseDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateExcuseDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPatch("{id}/accept")]
    public async Task<IActionResult> Accept(int id, [FromBody] AcceptExcuseDto dto)
    {
        var success = await _service.AcceptAsync(id, dto.IsAccepted);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

