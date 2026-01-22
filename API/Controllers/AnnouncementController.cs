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

    private int GetUserId()
    {
        var claim = User.FindFirst("userId")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDesc = true,
        [FromQuery] bool showInactive = false,
        [FromQuery] string? authorName = null,
        [FromQuery] int? targetRoleId = null)
    {
        var userId = GetUserId();
        var items = await _service.GetAllAsync(userId, search, sortBy, sortDesc, showInactive, authorName, targetRoleId);
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
        var authorId = GetUserId();
        if (authorId == 0) return Unauthorized("Nie udało się ustalić autora ogłoszenia");

        var entity = new Announcement
        {
            Title = dto.Title,
            Description = dto.Description,
            AuthorId = authorId
        };
        var result = await _service.CreateAsync(entity, dto.RoleIds);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAnnouncementDto dto)
    {
        var result = await _service.UpdateAsync(id, dto.Title, dto.Description, dto.RoleIds);
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

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = GetUserId();
        if (userId == 0) return Unauthorized();
        
        await _service.MarkAsReadAsync(id, userId);
        return NoContent();
    }
}

