using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ISchoolYearService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false);
        Task<object?> GetByIdAsync(int id);
        Task<IEnumerable<object>> GetSemestersAsync(int id);
        Task<SchoolYear> CreateAsync(SchoolYear entity);
        Task<SchoolYear?> UpdateAsync(int id, SchoolYear entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class SchoolYearService : ISchoolYearService
    {
        private readonly EduPlusDbContext _context;

        public SchoolYearService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false)
        {
            var query = _context.SchoolYears.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "startdate" => sortDesc ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate),
                "enddate" => sortDesc ? query.OrderByDescending(x => x.EndDate) : query.OrderBy(x => x.EndDate),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.StartDate) : query.OrderBy(x => x.StartDate)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.StartDate,
                    x.EndDate,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            return await _context.SchoolYears.AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.StartDate,
                    x.EndDate,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<object>> GetSemestersAsync(int id)
        {
            var semesters = await _context.Semesters.AsNoTracking()
                .Where(s => s.SchoolYearId == id)
                .OrderBy(s => s.StartDate)
                .ToListAsync();

            return semesters.Select((s, index) => new
            {
                s.Id,
                s.Name,
                Order = index + 1,
                s.StartDate,
                s.EndDate,
                s.IsActive
            });
        }

        public async Task<SchoolYear> CreateAsync(SchoolYear entity)
        {
            if (entity.StartDate >= entity.EndDate)
                throw new InvalidOperationException("Data rozpoczęcia musi być wcześniejsza niż data zakończenia");

            if (await _context.SchoolYears.AnyAsync(x => x.Name == entity.Name && x.IsActive))
                throw new InvalidOperationException("Taki rok szkolny już istnieje");

            entity.IsActive = true;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.SchoolYears.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<SchoolYear?> UpdateAsync(int id, SchoolYear entity)
        {
            if (id != entity.Id) return null;

            if (entity.StartDate >= entity.EndDate)
                throw new InvalidOperationException("Data rozpoczęcia musi być wcześniejsza niż data zakończenia");

            if (await _context.SchoolYears.AnyAsync(x => x.Name == entity.Name && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Taki rok szkolny już istnieje");

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
                if (!await _context.SchoolYears.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.SchoolYears.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.SchoolYears.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.SchoolYears.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
