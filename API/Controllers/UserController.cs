using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly SchoolDbContext _context;
    public UserController(SchoolDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserQueryDto query)
    {
        var dbQuery = _context.Users.AsNoTracking().AsQueryable();

        if (query.ShowInactive)
        {
            dbQuery = dbQuery.Where(u => !u.IsActive);
        }
        else
        {
            dbQuery = dbQuery.Where(u => u.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();
            dbQuery = dbQuery.Where(u => u.LastName.Contains(s) || u.FirstName.Contains(s) || u.Email.Contains(s));
        }

        dbQuery = query.SortBy?.ToLower() switch
        {
            "email" => query.SortDesc ? dbQuery.OrderByDescending(u => u.Email) : dbQuery.OrderBy(u => u.Email),
            "created" => query.SortDesc ? dbQuery.OrderByDescending(u => u.CreatedAt) : dbQuery.OrderBy(u => u.CreatedAt),
            "updated" => query.SortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt),
            "role" => query.SortDesc
                ? dbQuery.OrderByDescending(u => u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault())
                : dbQuery.OrderBy(u => u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()),
            _ => query.SortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt)
        };

        var users = await dbQuery
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Phone,
                u.Street,
                u.City,
                u.PostalCode,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
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
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Phone,
                u.Street,
                u.City,
                u.PostalCode,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
                UserRoles = u.UserRoles
                    .Where(ur => ur.IsActive)
                    .Select(ur => new {
                        ur.Id,
                        ur.UserId,
                        ur.RoleId,
                        Role = new { ur.Role.Id, ur.Role.Name }
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (await _context.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Podany email już istnieje w bazie");

        if (dto.RoleIds == null || !dto.RoleIds.Any())
            return BadRequest("Użytkownik musi mieć rolę");

        var entity = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Street = dto.Street,
            City = dto.City,
            PostalCode = dto.PostalCode,
            IsActive = dto.IsActive,
            Password = dto.Password,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserRoles = dto.RoleIds.Select(roleId => new UserRole
            {
                RoleId = roleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList()
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("Niezgodność ID");

        var dbUser = await _context.Users
            .Include(u => u.UserRoles)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (dbUser == null) return NotFound();

        dbUser.FirstName = dto.FirstName;
        dbUser.LastName = dto.LastName.Replace(" (nieaktywny)", "").Trim();
        dbUser.Email = dto.Email;
        dbUser.Phone = dto.Phone;
        dbUser.Street = dto.Street;
        dbUser.City = dto.City;
        dbUser.PostalCode = dto.PostalCode;
        dbUser.IsActive = dto.IsActive;

        if (!string.IsNullOrEmpty(dto.Password))
            dbUser.Password = dto.Password;

        dbUser.UpdatedAt = DateTime.UtcNow;

        var rolesToRemove = dbUser.UserRoles
            .Where(ur => ur.IsActive && !dto.RoleIds.Contains(ur.RoleId))
            .ToList();

        foreach (var role in rolesToRemove)
        {
            role.IsActive = false;
            role.UpdatedAt = DateTime.UtcNow;
        }

        foreach (var roleId in dto.RoleIds)
        {
            var existingRole = dbUser.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (existingRole == null)
            {
                dbUser.UserRoles.Add(new UserRole { RoleId = roleId, IsActive = true });
            }
            else if (!existingRole.IsActive)
            {
                existingRole.IsActive = true;
                existingRole.UpdatedAt = DateTime.UtcNow;
            }
        }

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