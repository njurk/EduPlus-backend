using BusinessLogic.Services;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _service;

    public AnnouncementController(IAnnouncementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] bool showInactive = false,
        [FromQuery] string? authorName = null)
    {
        var items = await _service.GetAllAsync(search, sortBy, sortDesc, showInactive, authorName);
        return Ok(items);
    }

    [HttpGet("authors")]
    public async Task<IActionResult> GetAuthors()
    {
        var result = await _service.GetAuthorsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(new AnnouncementDto
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            AuthorId = item.AuthorId,
            AuthorName = item.Author != null ? $"{item.Author.FirstName} {item.Author.LastName}" : string.Empty,
            IsActive = item.IsActive,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        var userIdClaim = User.FindFirst("userId")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var authorId))
        {
            return Unauthorized("Nie udało się ustalić autora ogłoszenia");
        }

        var entity = new Announcement
        {
            Title = dto.Title,
            Description = dto.Description,
            AuthorId = authorId
        };
        var result = await _service.CreateAsync(entity);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
    {
        var result = await _service.UpdateAsync(id, dto.Title, dto.Description);
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
        return NoContent();
    }
}

