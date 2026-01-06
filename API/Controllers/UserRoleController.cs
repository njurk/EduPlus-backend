using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CreateUserRoleDto
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public UserRoleController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userRoles = await _context.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .AsNoTracking()
            .Select(ur => new
            {
                ur.Id,
                ur.UserId,
                ur.RoleId,
                UserFirstName = ur.User.FirstName,
                UserLastName = ur.User.LastName,
                RoleName = ur.Role.Name
            })
            .ToListAsync();

        return Ok(userRoles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRoleDto dto)
    {
        var exists = await _context.UserRoles
            .AnyAsync(x => x.UserId == dto.UserId && x.RoleId == dto.RoleId);

        if (exists)
        {
            return BadRequest("Użytkownik już posiada tę rolę");
        }

        var entity = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.UserRoles.Add(entity);
        await _context.SaveChangesAsync();

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var roleToDelete = await _context.UserRoles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (roleToDelete == null) return NotFound();

        var activeRolesCount = await _context.UserRoles
            .CountAsync(ur => ur.UserId == roleToDelete.UserId && ur.Id != id);

        if (activeRolesCount == 0)
        {
            return BadRequest("Użytkownik musi mieć rolę");
        }

        _context.UserRoles.Remove(roleToDelete);

        await _context.SaveChangesAsync();
        return NoContent();
    }
}