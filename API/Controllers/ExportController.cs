using API.Services;
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

        [HttpGet("schedule/pdf")]
        public async Task<IActionResult> ExportSchedulePdf(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var bytes = await _service.ExportScheduleToPdfAsync(classId, yearId, semesterId);
            return File(bytes, "application/pdf", $"plan_lekcji_{classId}.pdf");
        }

        [HttpGet("schedule/xlsx")]
        public async Task<IActionResult> ExportScheduleXlsx(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var bytes = await _service.ExportScheduleToXlsxAsync(classId, yearId, semesterId);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"plan_lekcji_{classId}.xlsx");
        }

        [HttpGet("schedule/csv")]
        public async Task<IActionResult> ExportScheduleCsv(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var bytes = await _service.ExportScheduleToCsvAsync(classId, yearId, semesterId);
            return File(bytes, "text/csv", $"plan_lekcji_{classId}.csv");
        }
    }
}
