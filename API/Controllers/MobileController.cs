using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MobileController : ControllerBase
    {
        private readonly IMobileService _mobileService;

        public MobileController(IMobileService mobileService)
        {
            _mobileService = mobileService;
        }

        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _mobileService.GetScheduleAsync(GetUserId());
            if (result == null) return NotFound("Nie znaleziono planu lekcji");
            return Ok(result);
        }

        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades([FromQuery] int? semesterId = null)
        {
            var result = await _mobileService.GetGradesAsync(GetUserId(), semesterId);
            return Ok(result);
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance([FromQuery] int? semesterId = null)
        {
            var result = await _mobileService.GetAttendanceAsync(GetUserId(), semesterId);
            return Ok(result);
        }

        [HttpGet("announcements")]
        public async Task<IActionResult> GetAnnouncements()
        {
            var result = await _mobileService.GetAnnouncementsAsync(GetUserId());
            return Ok(result);
        }
    }
}
