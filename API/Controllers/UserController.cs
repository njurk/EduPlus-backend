using Data.Data.Entities;
using Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using BCrypt.Net;
using BusinessLogic.Services;

[ApiController]
[Route("api/[controller]")]
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
        var dbQuery = _context.Users.AsNoTracking().AsQueryable();

        dbQuery = query.ShowInactive ? dbQuery.Where(u => !u.IsActive) : dbQuery.Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();
            dbQuery = dbQuery.Where(u => u.LastName.Contains(s) || u.FirstName.Contains(s) || u.Email.Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(query.RoleName))
        {
            dbQuery = dbQuery.Where(u => u.UserRoles.Any(ur => ur.Role.Name == query.RoleName));
        }

        if (query.OnlyUnassignedParents)
        {
            dbQuery = dbQuery.Where(u =>
                u.UserRoles.Any(ur => ur.Role.Name == "Rodzic") &&
                !_context.ParentStudents.Any(ps => ps.ParentId == u.Id));
        }

        dbQuery = query.SortBy?.ToLower() switch
        {
            "email" => query.SortDesc ? dbQuery.OrderByDescending(u => u.Email) : dbQuery.OrderBy(u => u.Email),
            "created" => query.SortDesc ? dbQuery.OrderByDescending(u => u.CreatedAt) : dbQuery.OrderBy(u => u.CreatedAt),
            "updated" => query.SortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt),
            "role" => query.SortDesc
                ? dbQuery.OrderByDescending(u => u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault())
                : dbQuery.OrderBy(u => u.UserRoles.Select(ur => ur.Role.Name).FirstOrDefault()),
            "lastname" => query.SortDesc
                ? dbQuery.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName)
                : dbQuery.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
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
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
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
            UserRoles = user.UserRoles.Select(ur => new { ur.RoleId, Role = new { ur.Role.Id, ur.Role.Name } })
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
            UpdatedAt = DateTime.Now,
            UserRoles = dto.RoleIds.Select(roleId => new UserRole
            {
                RoleId = roleId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }).ToList()
        };

        if (dto.ChildIds != null && dto.ChildIds.Any())
        {
            foreach (var studentId in dto.ChildIds.Distinct())
            {
                _context.ParentStudents.Add(new ParentStudent
                {
                    Parent = entity,
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

        if (!string.IsNullOrEmpty(dto.Password))
            dbUser.Password = _passwordHashService.HashPassword(dto.Password);

        dbUser.UpdatedAt = DateTime.Now;

        var rolesToRemove = dbUser.UserRoles.Where(ur => !dto.RoleIds.Contains(ur.RoleId)).ToList();
        if (rolesToRemove.Any()) _context.UserRoles.RemoveRange(rolesToRemove);

        foreach (var roleId in dto.RoleIds)
        {
            if (!dbUser.UserRoles.Any(ur => ur.RoleId == roleId))
            {
                dbUser.UserRoles.Add(new UserRole { RoleId = roleId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            }
        }

        if (dto.ChildIds != null)
        {
            var currentRelations = await _context.ParentStudents.Where(ps => ps.ParentId == id).ToListAsync();
            var relationsToDelete = currentRelations.Where(r => !dto.ChildIds.Contains(r.StudentId)).ToList();
            if (relationsToDelete.Any()) _context.ParentStudents.RemoveRange(relationsToDelete);

            var existingStudentIds = currentRelations.Select(r => r.StudentId).ToList();
            var newStudentIds = dto.ChildIds.Where(id => !existingStudentIds.Contains(id)).Distinct();
            foreach (var studentId in newStudentIds)
            {
                _context.ParentStudents.Add(new ParentStudent { ParentId = id, StudentId = studentId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(dbUser);
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> Restore(int id)
    {
        var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        user.IsActive = true;

        if (user.LastName.EndsWith(" (nieaktywny)"))
        {
            user.LastName = user.LastName.Replace(" (nieaktywny)", "").Trim();
        }

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
            {
                item.LastName = $"{item.LastName} (nieaktywny)";
            }
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