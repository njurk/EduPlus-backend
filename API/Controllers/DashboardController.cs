using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        private static readonly DateTime _startTime = DateTime.Now;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            return Ok(await _service.GetSummaryAsync());
        }

        [HttpGet("attendance-chart")]
        public async Task<IActionResult> GetAttendanceChart()
        {
            return Ok(await _service.GetAttendanceChartAsync());
        }

        [HttpGet("uptime")]
        [AllowAnonymous]
        public IActionResult GetUptime()
        {
            var uptime = DateTime.Now - _startTime;
            return Ok(new
            {
                StartedAt = _startTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Uptime = $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s",
                UptimeSeconds = (int)uptime.TotalSeconds
            });
        }
    }
}