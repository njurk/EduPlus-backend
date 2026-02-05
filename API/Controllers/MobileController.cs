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

        [HttpGet("children")]
        public async Task<IActionResult> GetChildren()
        {
            var result = await _mobileService.GetChildrenAsync(GetUserId());
            return Ok(result);
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] int? studentId = null)
        {
            var result = await _mobileService.GetScheduleAsync(GetUserId(), studentId);
            if (result == null) return NotFound("Nie znaleziono planu lekcji");
            return Ok(result);
        }

        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades([FromQuery] int? studentId = null, [FromQuery] int? semesterId = null)
        {
            var result = await _mobileService.GetGradesAsync(GetUserId(), studentId, semesterId);
            return Ok(result);
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance([FromQuery] int? studentId = null, [FromQuery] int? semesterId = null, [FromQuery] string? date = null)
        {
            DateOnly? parsedDate = null;
            if (!string.IsNullOrEmpty(date) && DateOnly.TryParse(date, out var d))
            {
                parsedDate = d;
            }
            var result = await _mobileService.GetAttendanceAsync(GetUserId(), studentId, semesterId, parsedDate);
            return Ok(result);
        }

        [HttpGet("announcements")]
        public async Task<IActionResult> GetAnnouncements()
        {
            var result = await _mobileService.GetAnnouncementsAsync(GetUserId());
            return Ok(result);
        }

        [HttpGet("negative-attendances")]
        public async Task<IActionResult> GetNegativeAttendances([FromQuery] int? studentId = null, [FromQuery] int? semesterId = null)
        {
            var result = await _mobileService.GetNegativeAttendancesAsync(GetUserId(), studentId, semesterId);
            return Ok(result);
        }

        [HttpPost("excuse")]
        public async Task<IActionResult> CreateExcuse([FromQuery] int? studentId, [FromBody] Shared.DTOs.Mobile.CreateMobileExcuseDto dto)
        {
            await _mobileService.CreateExcuseAsync(GetUserId(), studentId, dto);
            return Ok();
        }

        [HttpGet("semesters")]
        public async Task<IActionResult> GetSemesters()
        {
            var result = await _mobileService.GetSemestersAsync();
            return Ok(result);
        }
    }
}
