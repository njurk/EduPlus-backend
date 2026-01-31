using API.Helpers;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeService _service;
        private readonly IConfiguration _configuration;
        private readonly IBadgeNotificationService _notificationService;
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public BadgeController(IBadgeService service, IConfiguration configuration, IBadgeNotificationService notificationService)
        {
            _service = service;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        [HttpGet("unread-counts")]
        public async Task<IActionResult> GetUnreadCounts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();
            return Ok(await _service.GetUnreadCountsAsync(userId));
        }

        [AllowAnonymous]
        [HttpGet("unread-counts/stream")]
        public async Task StreamUnreadCounts([FromQuery] string token, CancellationToken ct)
        {
            var userId = JwtHelper.ValidateTokenAndGetUserId(token, _configuration);
            if (userId == null) { Response.StatusCode = 401; return; }

            Response.Headers["Content-Type"] = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";

            var signal = new SemaphoreSlim(0);
            void OnChange(int id) { if (id == userId) signal.Release(); }
            _notificationService.OnBadgeChanged += OnChange;

            try
            {
                await SendCounts(userId.Value, ct);
                while (!ct.IsCancellationRequested)
                {
                    await signal.WaitAsync(ct);
                    await SendCounts(userId.Value, ct);
                }
            }
            catch (OperationCanceledException) { }
            finally { _notificationService.OnBadgeChanged -= OnChange; }
        }

        private async Task SendCounts(int userId, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(await _service.GetUnreadCountsAsync(userId), JsonOptions);
            await Response.WriteAsync($"data: {json}\n\n", ct);
            await Response.Body.FlushAsync(ct);
        }
    }
}
