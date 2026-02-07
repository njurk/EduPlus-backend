using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IAttendanceTypeService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<AttendanceType?> UpdateAsync(int id, AttendanceType entity);
    }

    public class AttendanceTypeService : IAttendanceTypeService
    {
        private readonly EduPlusDbContext _context;

        public AttendanceTypeService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.AttendanceTypes.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(r => !r.IsActive) : query.Where(r => r.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(x => x.Name.Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Slug,
                    x.ShortCode,
                    x.ColorHex,
                    x.IsNegative,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<AttendanceType?> UpdateAsync(int id, AttendanceType entity)
        {
            if (id != entity.Id) return null;

            if (await _context.AttendanceTypes.AnyAsync(x => x.Name == entity.Name && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Taka nazwa frekwencji już istnieje");

            if (await _context.AttendanceTypes.AnyAsync(x => x.ShortCode == entity.ShortCode && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Taki skrót już istnieje");

            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.AttendanceTypes.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }
    }
}
