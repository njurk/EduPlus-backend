using Data.Data;
using Data.Data.CMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PageController : ControllerBase
    {
        private readonly EduPlusDbContext _context;

        public PageController(EduPlusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Pages.AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode(403, new { message = "Dodawanie stron nie jest dozwolone" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(403, new { message = "Usuwanie stron nie jest dozwolone" });
        }
    }
}
