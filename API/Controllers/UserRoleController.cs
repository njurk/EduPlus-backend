using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                ur.IsActive,
                UserFirstName = ur.User.FirstName,
                UserLastName = ur.User.LastName,
                RoleName = ur.Role.Name
            })
            .ToListAsync();

        return Ok(userRoles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRole entity)
    {
        var exists = await _context.UserRoles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.UserId == entity.UserId && x.RoleId == entity.RoleId);

        if (exists != null)
        {
            if (exists.IsActive) return BadRequest("Rola jest już przypisana dla tego użytkownika");

            exists.IsActive = true;
            exists.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(exists);
        }

        _context.UserRoles.Add(entity);

        await _context.SaveChangesAsync();

        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.UserRoles.FindAsync(id);
        if (item == null) return NotFound();

        item.IsActive = false;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}