using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public UserController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Phone,
                u.IsActive,
                UserRoles = u.UserRoles.Select(ur => new {
                    ur.Id,
                    ur.UserId,
                    ur.RoleId,
                    Role = new { ur.Role.Id, ur.Role.Name }
                }).ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Id == id);

        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User entity)
    {
        if (await _context.Users.IgnoreQueryFilters()
            .AnyAsync(u => u.Email == entity.Email)) 
            return BadRequest("Podany email już istnieje w bazie");

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User entity)
    {
        if (id != entity.Id) return BadRequest("Nie udało się zaktualizować użytkownika");

        var dbUser = await _context.Users.FindAsync(id);
        if (dbUser == null || !dbUser.IsActive) return NotFound();

        dbUser.FirstName = entity.FirstName;
        dbUser.LastName = entity.LastName;
        dbUser.Email = entity.Email;
        dbUser.Phone = entity.Phone;
        if (!string.IsNullOrEmpty(entity.Password)) dbUser.Password = entity.Password;
        dbUser.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(dbUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Users.FindAsync(id);
        if (item == null || !item.IsActive) return NotFound();

        item.LastName = $"{item.LastName} (nieaktywny)";
        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}