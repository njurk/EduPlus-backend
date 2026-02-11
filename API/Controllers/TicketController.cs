using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;
        private readonly IEmailService _emailService;
        private readonly IBadgeNotificationService _badgeNotification;

        public TicketController(ITicketService service, IEmailService emailService, IBadgeNotificationService badgeNotification)
        {
            _service = service;
            _emailService = emailService;
            _badgeNotification = badgeNotification;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, bool? showClosed = null, string? search = null, string? sortBy = "createdAt", bool sortDesc = true, int? reasonId = null)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize, showClosed, search, sortBy, sortDesc, reasonId);
            return Ok(result);
        }

        [HttpGet("submitters")]
        public async Task<IActionResult> GetSubmitters()
        {
            var result = await _service.GetSubmittersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto, _emailService);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/close")]
        public async Task<IActionResult> Close(int id, [FromBody] CloseTicketDto dto)
        {
            var success = await _service.CloseAsync(id, dto, _emailService);
            if (!success) return NotFound();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
                _badgeNotification.NotifyBadgeChanged(userId);

            return NoContent();
        }
    }
}
