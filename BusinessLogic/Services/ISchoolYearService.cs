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
        Task<IEnumerable<object>> GetAllAsync();
        Task<object?> GetByIdAsync(int id);
        Task<IEnumerable<object>> GetSemestersAsync(int id);
        Task<SchoolYear> CreateAsync(SchoolYear entity);
        Task<SchoolYear?> UpdateAsync(int id, SchoolYear entity);
        Task<bool> DeleteAsync(int id);
    }

    public class SchoolYearService : ISchoolYearService
    {
        private readonly EduPlusDbContext _context;

        public SchoolYearService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.SchoolYears.AsNoTracking()
                .OrderByDescending(x => x.StartDate)
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
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.SchoolYears.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<SchoolYear?> UpdateAsync(int id, SchoolYear entity)
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
                if (!await _context.SchoolYears.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.SchoolYears.FindAsync(id);
            if (item == null) return false;

            _context.SchoolYears.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
