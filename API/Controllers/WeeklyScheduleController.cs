using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
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

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetTeacherSchedule(int teacherId, [FromQuery] int? semesterId = null)
        {
            var schedule = await _service.GetScheduleForTeacherAsync(teacherId, semesterId);
            return Ok(schedule);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableForDate([FromQuery] DateTime date, [FromQuery] int? classId = null, [FromQuery] int? teacherId = null, [FromQuery] int? semesterId = null)
        {
            var schedule = await _service.GetAvailableForDateAsync(date, classId, teacherId, semesterId);
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] WeeklyScheduleDto dto)
        {
            try
            {
                var result = await _service.CreateOrUpdateAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearSchedule([FromQuery] int classId, [FromQuery] int semesterId)
        {
            var count = await _service.ClearScheduleAsync(classId, semesterId);
            return Ok(new { deletedCount = count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}