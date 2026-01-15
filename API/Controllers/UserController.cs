using Shared.DTOs;
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
    private readonly EduPlusDbContext _context;

    private readonly IPasswordHashService _passwordHashService;

    public UserController(EduPlusDbContext context, IPasswordHashService passwordHashService)
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
            .FirstOrDefaultAsync();

        if (user == null) return NotFound();

        var relations = await _context.ParentStudents
            .AsNoTracking()
            .Where(ps => ps.ParentId == id || ps.StudentId == id)
            .Include(ps => ps.Parent)
            .Include(ps => ps.Student)
            .ToListAsync();

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
            ChildIds = relations.Where(ps => ps.ParentId == id).Select(ps => ps.StudentId).ToList(),
            ParentIds = relations.Where(ps => ps.StudentId == id).Select(ps => ps.ParentId).ToList(),
            Relations = relations.Select(ps => 
                ps.ParentId == id 
                    ? $"{ps.Student.LastName} {ps.Student.FirstName} (Uczeñ)" 
                    : $"{ps.Parent.LastName} {ps.Parent.FirstName} (Rodzic)"
            ).ToList()
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (await _context.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Podany email ju¿ istnieje w bazie");

        if (dto.RoleIds == null || !dto.RoleIds.Any())
            return BadRequest("U¿ytkownik musi mieæ rolê");

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
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Street = dto.Street;
        user.City = dto.City;
        user.PostalCode = dto.PostalCode;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.Now;

        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(currentUserId, out int uid))
        {
            user.ModifiedByUserId = uid;
        }

        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.Password = _passwordHashService.HashPassword(dto.Password);
        }

        if (dto.RoleIds != null)
        {
            var currentRoles = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var toAdd = dto.RoleIds.Except(currentRoles).ToList();
            var toRemove = currentRoles.Except(dto.RoleIds).ToList();

            foreach (var rid in toRemove)
            {
                var r = user.UserRoles.First(ur => ur.RoleId == rid);
                _context.UserRoles.Remove(r);
            }

            foreach (var rid in toAdd)
            {
                user.UserRoles.Add(new UserRole { RoleId = rid, UserId = user.Id });
            }
        }

        if (dto.ParentIds != null && dto.ParentIds.Any())
        {
            var currentParents = await _context.ParentStudents
                .Where(ps => ps.StudentId == id)
                .ToListAsync();
            
            var currentParentIds = currentParents.Select(ps => ps.ParentId).ToList();
            var toAdd = dto.ParentIds.Except(currentParentIds).ToList();
            var toRemove = currentParentIds.Except(dto.ParentIds).ToList();

            foreach (var pid in toRemove)
            {
                var rel = currentParents.First(ps => ps.ParentId == pid);
                _context.ParentStudents.Remove(rel);
            }
            foreach (var pid in toAdd)
            {
                _context.ParentStudents.Add(new ParentStudent { StudentId = id, ParentId = pid });
            }
        }
        
        if (dto.ChildIds != null && dto.ChildIds.Any()) 
        {
                var currentChilds = await _context.ParentStudents
                .Where(ps => ps.ParentId == id)
                .ToListAsync();

            var currentChildIds = currentChilds.Select(ps => ps.StudentId).ToList();
            var toAdd = dto.ChildIds.Except(currentChildIds).ToList();
            var toRemove = currentChildIds.Except(dto.ChildIds).ToList();
            
            foreach (var sid in toRemove)
            {
                var rel = currentChilds.First(ps => ps.StudentId == sid);
                _context.ParentStudents.Remove(rel); 
            }
            foreach (var sid in toAdd)
            {
                _context.ParentStudents.Add(new ParentStudent { ParentId = id, StudentId = sid });
            }
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDto dto)
    {
        var dbUser = await _context.Users.FindAsync(id);
        if (dbUser == null) return NotFound();

        if (!_passwordHashService.VerifyPassword(dto.CurrentPassword, dbUser.Password))
            return BadRequest("Aktualne has˜o jest nieprawid˜owe.");

        dbUser.Password = _passwordHashService.HashPassword(dto.NewPassword);
        dbUser.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Has˜o zosta˜o zmienione pomy˜lnie." });
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

        return Ok(new { message = "U¿ytkownik przywrócony", id = user.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Users.FindAsync(id);
        if (item == null) return NotFound();

        if (item.IsActive)
        {
            if (!item.LastName.EndsWith(" (usuniêty)"))
                item.LastName = $"{item.LastName} (usuniêty)";

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }
}
