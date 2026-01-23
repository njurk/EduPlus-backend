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

        [HttpGet("schedule/pdf")]
        public async Task<IActionResult> ExportSchedulePdf(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var result = await _service.ExportScheduleToPdfAsync(classId, yearId, semesterId);
            return File(result.Data, "application/pdf", result.FileName);
        }

        [HttpGet("schedule/xlsx")]
        public async Task<IActionResult> ExportScheduleXlsx(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var result = await _service.ExportScheduleToXlsxAsync(classId, yearId, semesterId);
            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
        }

        [HttpGet("schedule/csv")]
        public async Task<IActionResult> ExportScheduleCsv(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var result = await _service.ExportScheduleToCsvAsync(classId, yearId, semesterId);
            return File(result.Data, "text/csv", result.FileName);
        }

        [HttpGet("schedule/docx")]
        public async Task<IActionResult> ExportScheduleDocx(
            [FromQuery] int classId,
            [FromQuery] int? yearId = null,
            [FromQuery] int? semesterId = null)
        {
            var result = await _service.ExportScheduleToDocxAsync(classId, yearId, semesterId);
            return File(result.Data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", result.FileName);
        }
    }
}
