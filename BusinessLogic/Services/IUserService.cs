using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Data.Data.EntitiesForView;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<PaginatedResponse<UserListView>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 20,
            string? search = null,
            string? sortBy = null,
            bool sortDesc = true,
            bool showInactive = false,
            bool onlyUnassignedParents = false,
            int? roleLevel = null);
        Task<object?> GetByIdAsync(int id);
        Task<User> CreateAsync(UserCreateDto dto);
        Task<bool> UpdateAsync(int id, UserUpdateDto dto, int? modifiedByUserId);
        Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<bool> RestoreAsync(int id);
        Task<bool> DeleteAsync(int id);
    }

    public class UserService : IUserService
    {
        private readonly EduPlusDbContext _context;
        private readonly IPasswordHashService _passwordHashService;

        public UserService(EduPlusDbContext context, IPasswordHashService passwordHashService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
        }

        public async Task<PaginatedResponse<UserListView>> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 20,
            string? search = null,
            string? sortBy = null,
            bool sortDesc = true,
            bool showInactive = false,
            bool onlyUnassignedParents = false,
            int? roleLevel = null)
        {
            var dbQuery = _context.UserList.AsNoTracking().AsQueryable();

            dbQuery = showInactive ? dbQuery.Where(u => !u.IsActive) : dbQuery.Where(u => u.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                dbQuery = dbQuery.Where(u => u.LastName.Contains(s) || u.FirstName.Contains(s) || u.Email.Contains(s));
            }

            if (roleLevel.HasValue)
            {
                var roleName = await _context.Roles
                    .Where(r => r.Level == roleLevel.Value)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();
                if (!string.IsNullOrEmpty(roleName))
                    dbQuery = dbQuery.Where(u => u.RoleNames.Contains(roleName));
            }

            if (onlyUnassignedParents)
            {
                dbQuery = dbQuery.Where(u => u.IsUnassignedParent);
            }

            dbQuery = sortBy?.ToLower() switch
            {
                "email" => sortDesc ? dbQuery.OrderByDescending(u => u.Email) : dbQuery.OrderBy(u => u.Email),
                "created" => sortDesc ? dbQuery.OrderByDescending(u => u.CreatedAt) : dbQuery.OrderBy(u => u.CreatedAt),
                "updated" => sortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt),
                "role" => sortDesc ? dbQuery.OrderByDescending(u => u.RoleNames) : dbQuery.OrderBy(u => u.RoleNames),
                "name" or "lastname" => sortDesc
                    ? dbQuery.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName)
                    : dbQuery.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
                _ => sortDesc ? dbQuery.OrderByDescending(u => u.UpdatedAt) : dbQuery.OrderBy(u => u.UpdatedAt)
            };

            var totalCount = await dbQuery.CountAsync();

            var data = await dbQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<UserListView>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            var relations = await _context.ParentStudents
                .AsNoTracking()
                .Where(ps => ps.ParentId == id || ps.StudentId == id)
                .Include(ps => ps.Parent)
                .Include(ps => ps.Student)
                .ToListAsync();

            return new
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
                        ? $"{ps.Student.LastName} {ps.Student.FirstName} (Uczeń)"
                        : $"{ps.Parent.LastName} {ps.Parent.FirstName} (Rodzic)"
                ).ToList()
            };
        }

        public async Task<User> CreateAsync(UserCreateDto dto)
        {
            if (await _context.Users.IgnoreQueryFilters().AnyAsync(u => u.Email == dto.Email))
                throw new InvalidOperationException("Podany email już istnieje w bazie");

            if (dto.RoleIds == null || !dto.RoleIds.Any())
                throw new InvalidOperationException("Uytkownik musi mieć rolę");

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

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto, int? modifiedByUserId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Street = dto.Street;
            user.City = dto.City;
            user.PostalCode = dto.PostalCode;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.Now;
            user.ModifiedByUserId = modifiedByUserId;

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

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null) return false;

            if (!_passwordHashService.VerifyPassword(currentPassword, dbUser.Password))
                throw new UnauthorizedAccessException("Aktualne hasło jest nieprawidłowe.");

            dbUser.Password = _passwordHashService.HashPassword(newPassword);
            dbUser.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return false;

            user.IsActive = true;
            if (user.LastName.EndsWith(" (usunięty)"))
                user.LastName = user.LastName.Replace(" (usunięty)", "").Trim();

            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Users.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                if (!item.LastName.EndsWith(" (usunięty)"))
                    item.LastName = $"{item.LastName} (usunięty)";

                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
