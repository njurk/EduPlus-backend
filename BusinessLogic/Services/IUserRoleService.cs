using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc);
        Task<UserRole> CreateAsync(UserRole entity);
        Task<bool> DeleteAsync(int id);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly EduPlusDbContext _context;

        public UserRoleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc)
        {
            var query = _context.UserRoles.AsNoTracking()
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .Where(ur => ur.User.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(ur =>
                    ur.User.LastName.ToLower().Contains(s) ||
                    ur.User.FirstName.ToLower().Contains(s) ||
                    ur.Role.Name.ToLower().Contains(s));
            }

            var projected = query.Select(ur => new
            {
                ur.Id,
                ur.UserId,
                ur.RoleId,
                UserName = ur.User.LastName + " " + ur.User.FirstName,
                RoleName = ur.Role.Name,
                ur.CreatedAt
            });

            projected = sortBy?.ToLower() switch
            {
                "username" => sortDesc ? projected.OrderByDescending(x => x.UserName) : projected.OrderBy(x => x.UserName),
                "rolename" => sortDesc ? projected.OrderByDescending(x => x.RoleName) : projected.OrderBy(x => x.RoleName),
                _ => sortDesc ? projected.OrderByDescending(x => x.CreatedAt) : projected.OrderBy(x => x.CreatedAt)
            };

            return await projected.ToListAsync();
        }

        public async Task<UserRole> CreateAsync(UserRole entity)
        {
            if (await _context.UserRoles.AnyAsync(ur => ur.UserId == entity.UserId && ur.RoleId == entity.RoleId))
                throw new InvalidOperationException("Użytkownik ma już przypisaną tę rolę");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.UserRoles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.UserRoles.FindAsync(id);
            if (item == null) return false;

            _context.UserRoles.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
