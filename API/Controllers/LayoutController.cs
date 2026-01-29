using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LayoutController : ControllerBase
    {
        private readonly ILayoutService _service;

        public LayoutController(ILayoutService service)
        {
            _service = service;
        }

        [HttpGet("unread-counts")]
        public async Task<IActionResult> GetUnreadCounts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            return Ok(await _service.GetUnreadCountsAsync(userId));
        }
    }
}
