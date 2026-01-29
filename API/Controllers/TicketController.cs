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
        private readonly ITicketReadService _ticketReadService;

        public TicketController(ITicketService service, IEmailService emailService, ITicketReadService ticketReadService)
        {
            _service = service;
            _emailService = emailService;
            _ticketReadService = ticketReadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, bool? showClosed = null, string? search = null, string? sortBy = "createdAt", bool sortDesc = true, int? reasonId = null)
        {
            int? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var parsedUserId))
                userId = parsedUserId;

            var result = await _service.GetAllAsync(pageNumber, pageSize, showClosed, search, sortBy, sortDesc, reasonId, userId);
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
            var result = await _service.CreateAsync(dto, _emailService);
            return Ok(result);
        }

        [HttpPatch("{id}/close")]
        public async Task<IActionResult> Close(int id, [FromBody] CloseTicketDto dto)
        {
            var success = await _service.CloseAsync(id, dto, _emailService);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            await _ticketReadService.MarkAsReadAsync(id, userId);
            return NoContent();
        }
    }
}


