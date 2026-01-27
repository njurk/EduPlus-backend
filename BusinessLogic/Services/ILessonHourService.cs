using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ILessonHourService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<LessonHour> CreateAsync(LessonHour entity);
        Task<LessonHour?> UpdateAsync(int id, LessonHour entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class LessonHourService : ILessonHourService
    {
        private readonly EduPlusDbContext _context;

        public LessonHourService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.LessonHours.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(x => x.OrderNumber.ToString().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "ordernumber" => sortDesc ? query.OrderByDescending(x => x.OrderNumber) : query.OrderBy(x => x.OrderNumber),
                "starttime" => sortDesc ? query.OrderByDescending(x => x.StartTime) : query.OrderBy(x => x.StartTime),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.OrderNumber) : query.OrderBy(x => x.OrderNumber)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.OrderNumber,
                    x.StartTime,
                    x.EndTime,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<LessonHour> CreateAsync(LessonHour entity)
        {
            entity.IsActive = true;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.LessonHours.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<LessonHour?> UpdateAsync(int id, LessonHour entity)
        {
            if (id != entity.Id) return null;

            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            entity.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.LessonHours.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.LessonHours.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.LessonHours.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.LessonHours.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
