using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PageContentController : ControllerBase
    {
        private readonly IPageContentService _service;

        public PageContentController(IPageContentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPageId(int pageId, [FromQuery] string? search = null)
        {
            var result = await _service.GetByPageIdAsync(pageId, search);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("by-label/{pageLabel}")]
        public async Task<IActionResult> GetByPageLabel(string pageLabel)
        {
            var result = await _service.GetByPageLabelAsync(pageLabel);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string newValue)
        {
            try
            {
                var result = await _service.UpdateAsync(id, newValue);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode(403);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(403);
        }
    }
}