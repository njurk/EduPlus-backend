using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ISemesterService
    {
        Task<IEnumerable<object>> GetAllAsync(int? schoolYearId);
        Task<Semester> CreateAsync(Semester entity);
        Task<Semester?> UpdateAsync(int id, Semester entity);
        Task<bool> DeleteAsync(int id);
    }

    public class SemesterService : ISemesterService
    {
        private readonly EduPlusDbContext _context;

        public SemesterService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(int? schoolYearId)
        {
            var query = _context.Semesters.AsNoTracking().AsQueryable();

            if (schoolYearId.HasValue)
                query = query.Where(x => x.SchoolYearId == schoolYearId.Value);

            return await query
                .OrderBy(x => x.StartDate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.SchoolYearId,
                    x.StartDate,
                    x.EndDate,
                    x.IsActive
                })
                .ToListAsync();
        }

        public async Task<Semester> CreateAsync(Semester entity)
        {
            if (entity.StartDate >= entity.EndDate)
                throw new InvalidOperationException("Data rozpoczęcia semestru musi być wcześniejsza niż data zakończenia");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.Semesters.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Semester?> UpdateAsync(int id, Semester entity)
        {
            if (id != entity.Id) return null;

            if (entity.StartDate >= entity.EndDate)
                throw new InvalidOperationException("Data rozpoczęcia semestru musi być wcześniejsza niż data zakończenia");

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
                if (!await _context.Semesters.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Semesters.FindAsync(id);
            if (item == null) return false;

            _context.Semesters.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
