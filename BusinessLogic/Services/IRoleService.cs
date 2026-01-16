using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search);
    }

    public class RoleService : IRoleService
    {
        private readonly EduPlusDbContext _context;

        public RoleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search)
        {
            var query = _context.Roles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            return await query
                .OrderBy(x => x.Id)
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
    }
}
