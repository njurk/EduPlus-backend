using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ILessonStatusService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<LessonStatus?> UpdateAsync(int id, LessonStatus entity);
    }

    public class LessonStatusService : ILessonStatusService
    {
        private readonly EduPlusDbContext _context;

        public LessonStatusService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.LessonStatuses.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Slug,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<LessonStatus?> UpdateAsync(int id, LessonStatus entity)
        {
            if (id != entity.Id) return null;

            if (await _context.LessonStatuses.AnyAsync(x => x.Name == entity.Name && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Ten status lekcji już istnieje");

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
                if (!await _context.LessonStatuses.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }
    }
}
