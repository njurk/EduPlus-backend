using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Odkomentować w produkcji
    public class WeeklyScheduleController : ControllerBase
    {
        private readonly IWeeklyScheduleService _service;

        public WeeklyScheduleController(IWeeklyScheduleService service)
        {
            _service = service;
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> GetSchedule(int classId, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateFrom == default) dateFrom = DateTime.Today; // Domyślnie chociaż dzisiaj, ale frontend powinien wysyłać
            if (dateTo == default) dateTo = dateFrom.AddDays(7);

            var schedule = await _service.GetScheduleForClassAsync(classId, dateFrom, dateTo);
            return Ok(schedule);
        }
    }
}
