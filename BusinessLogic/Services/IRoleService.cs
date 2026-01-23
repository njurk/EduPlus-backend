using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy = null, bool sortDesc = true);
        Task<bool> UpdateAsync(int id, RoleUpdateDto dto);
    }

    public class RoleService : IRoleService
    {
        private readonly EduPlusDbContext _context;

        public RoleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy = null, bool sortDesc = true)
        {
            var query = _context.Roles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "level" => sortDesc ? query.OrderByDescending(x => x.Level) : query.OrderBy(x => x.Level),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => query.OrderBy(x => x.Id)
            };

            return await query
                .Select(x => new { 
                    x.Id, 
                    x.Name, 
                    x.Level,
                    x.Description,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = x.ModifiedByUserId != null 
                        ? _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()
                        : "System"
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int id, RoleUpdateDto dto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            role.Name = dto.Name;
            role.Description = dto.Description;
            role.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

