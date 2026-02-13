using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _service;

        public TemplateController(ITemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTemplates()
        {
            return Ok(_service.GetTemplates());
        }

        [HttpGet("{id}/content")]
        public async Task<IActionResult> GetContent(string id)
        {
            var html = await _service.GetTemplateContentAsync(id);
            return Ok(new { html });
        }

        [HttpPost("{id}/export")]
        public async Task<IActionResult> Export(string id, [FromBody] ExportRequest request)
        {
            var bytes = await _service.ExportAsync(id, request.Html, request.Placeholders);
            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{id}.docx");
        }
    }

    public class ExportRequest
    {
        public string Html { get; set; } = "";
        public Dictionary<string, string> Placeholders { get; set; } = new();
    }
}
