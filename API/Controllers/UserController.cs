using API.DTOs;
using BusinessLogic.Services;
using Data.Data;
using Data.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly SchoolDbContext _context;

    private readonly IPasswordHashService _passwordHashService;

    public UserController(SchoolDbContext context, IPasswordHashService passwordHashService)
    {
        _context = context;
        _passwordHashService = passwordHashService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserQueryDto query)
    {
        var dbQuery = _context.UserList.AsNoTracking().AsQueryable();

        dbQuery = query.ShowInactive ? dbQuery.Where(u => !u.IsActive) : dbQuery.Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();
            dbQuery = dbQuery.Where(u => u.LastName.Contains(s) || u.FirstName.Contains(s) || u.Email.Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(query.RoleName))
        {
            dbQuery = dbQuery.Where(u => u.RoleNames.Contains(query.RoleName));
        }

        if (query.OnlyUnassignedParents)
        {
            dbQuery = dbQuery.Where(u => u.IsUnassignedParent);
        }

        dbQuery = query.SortBy?.ToLower() switch
        {
            "email" => query.SortDesc ? dbQuery.OrderByDescending(u => u.Email) : dbQuery.OrderBy(u => u.Email),
            "created" => query.SortDesc ? dbQuery.OrderByDescending(u => u.CreatedAt) : dbQuery.OrderBy(u => u.CreatedAt),
            "updated" => query.SortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt),
            "role" => query.SortDesc ? dbQuery.OrderByDescending(u => u.RoleNames) : dbQuery.OrderBy(u => u.RoleNames),
            "lastname" => query.SortDesc
                ? dbQuery.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName)
                : dbQuery.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
            _ => query.SortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt)
        };

        return Ok(await dbQuery.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.ParentStudents)
            .FirstOrDefaultAsync();

        if (user == null) return NotFound();

        return Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Phone,
            user.Street,
            user.City,
            user.PostalCode,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt,
            UserRoles = user.UserRoles.Select(ur => new { ur.RoleId, Role = new { ur.Role.Id, ur.Role.Name } }),
            ChildIds = user.ParentStudents.Select(pr => pr.StudentId).ToList()
        });
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
            IsActive = true,
            Password = _passwordHashService.HashPassword(dto.Password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        foreach (var roleId in dto.RoleIds)
        {
            entity.UserRoles.Add(new UserRole { RoleId = roleId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
        }

        if (dto.ChildIds != null && dto.ChildIds.Any())
        {
            foreach (var studentId in dto.ChildIds.Distinct())
            {
                entity.ParentStudents.Add(new ParentStudent
                {
                    StudentId = studentId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
        }

        _context.Users.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        var dbUser = await _context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.ParentStudents)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (dbUser == null) return NotFound();

        dbUser.FirstName = dto.FirstName;
        dbUser.LastName = dto.LastName;
        dbUser.Email = dto.Email;
        dbUser.Phone = dto.Phone;
        dbUser.Street = dto.Street;
        dbUser.City = dto.City;
        dbUser.PostalCode = dto.PostalCode;
        dbUser.IsActive = dto.IsActive;
        dbUser.UpdatedAt = DateTime.Now;

        if (!string.IsNullOrEmpty(dto.Password))
            dbUser.Password = _passwordHashService.HashPassword(dto.Password);

        var rolesToRemove = dbUser.UserRoles.Where(ur => !dto.RoleIds.Contains(ur.RoleId)).ToList();
        foreach (var role in rolesToRemove) dbUser.UserRoles.Remove(role);

        foreach (var roleId in dto.RoleIds)
        {
            if (!dbUser.UserRoles.Any(ur => ur.RoleId == roleId))
                dbUser.UserRoles.Add(new UserRole { RoleId = roleId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
        }

        if (dto.ChildIds != null)
        {
            var relationsToDelete = dbUser.ParentStudents
                .Where(pr => !dto.ChildIds.Contains(pr.StudentId))
                .ToList();

            foreach (var rel in relationsToDelete)
            {
                dbUser.ParentStudents.Remove(rel);
            }

            var existingStudentIds = dbUser.ParentStudents.Select(pr => pr.StudentId).ToList();
            var newStudentIds = dto.ChildIds.Where(id => !existingStudentIds.Contains(id)).Distinct();

            foreach (var studentId in newStudentIds)
            {
                dbUser.ParentStudents.Add(new ParentStudent
                {
                    ParentId = id,
                    StudentId = studentId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(dbUser);
    }

    [HttpPatch("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto dto)
    {
        var dbUser = await _context.Users.FindAsync(id);
        if (dbUser == null) return NotFound();

        if (!_passwordHashService.VerifyPassword(dto.CurrentPassword, dbUser.Password))
            return BadRequest("Aktualne hasło jest nieprawidłowe.");

        dbUser.Password = _passwordHashService.HashPassword(dto.NewPassword);
        dbUser.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Hasło zostało zmienione pomyślnie." });
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        user.IsActive = true;
        if (user.LastName.EndsWith(" (nieaktywny)"))
            user.LastName = user.LastName.Replace(" (nieaktywny)", "").Trim();

        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Użytkownik przywrócony", id = user.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Users.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            if (!item.LastName.EndsWith(" (nieaktywny)"))
                item.LastName = $"{item.LastName} (nieaktywny)";

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;
        }
        else
        {
            _context.Users.Remove(item);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
