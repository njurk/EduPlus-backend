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
    public class TargetController : ControllerBase
    {
        private readonly EduPlusDbContext _context;

        public TargetController(EduPlusDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Set<Target>().AsNoTracking().ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create()
        {
            return StatusCode(403, new { message = "Dodawanie targetów nie jest dozwolone" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            return StatusCode(403, new { message = "Modyfikacja targetów nie jest dozwolone" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(403, new { message = "Usuwanie targetów nie jest dozwolone" });
        }
    }
}
