using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeeklyScheduleController : ControllerBase
    {
        private readonly IWeeklyScheduleService _service;

        public WeeklyScheduleController(IWeeklyScheduleService service)
        {
            _service = service;
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetSchedule(int classId, [FromQuery] int? semesterId = null)
        {
            var schedule = await _service.GetScheduleForClassAsync(classId, semesterId);
            return Ok(schedule);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableForDate([FromQuery] DateTime date, [FromQuery] int? classId = null, [FromQuery] int? teacherId = null, [FromQuery] int? semesterId = null)
        {
            var schedule = await _service.GetAvailableForDateAsync(date, classId, teacherId, semesterId);
            return Ok(schedule);
        }
    }
}