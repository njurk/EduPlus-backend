using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PageController : ControllerBase
{
    private readonly IPageService _service;

    public PageController(IPageService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int? targetId = null)
    {
        return Ok(await _service.GetAllAsync(targetId));
    }
}