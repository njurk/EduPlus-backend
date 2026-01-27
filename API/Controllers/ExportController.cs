using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _service;

        public ExportController(IExportService service)
        {
            _service = service;
        }

        [HttpGet("schedule/{format}")]
        public async Task<IActionResult> ExportSchedule(
            string format,
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var result = await _service.ExportScheduleAsync(classId, yearId, semesterId, format);
            return File(result.Data, result.ContentType, result.FileName);
        }

        [HttpGet("grades/{format}")]
        public async Task<IActionResult> ExportGrades(
            string format,
            [FromQuery] int classId,
            [FromQuery] int? subjectId = null,
            [FromQuery] int? semesterId = null,
            [FromQuery] int? schoolYearId = null,
            [FromQuery] int? studentId = null)
        {
            var result = await _service.ExportGradesAsync(classId, subjectId ?? 0, semesterId ?? 0, schoolYearId ?? 0, studentId, format);
            return File(result.Data, result.ContentType, result.FileName);
        }
    }
}
