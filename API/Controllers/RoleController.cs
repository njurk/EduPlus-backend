using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly SchoolDbContext _context;

    public RoleController(SchoolDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var query = _context.Roles.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(r => r.Name.Contains(s) || (r.Description != null && r.Description.Contains(s)));
        }

        query = query.OrderByDescending(r => r.Level).ThenBy(r => r.Name);

        return Ok(await query.ToListAsync());
    }
}