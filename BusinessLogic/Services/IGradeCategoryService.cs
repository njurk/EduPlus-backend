using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IGradeCategoryService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<GradeCategory> CreateAsync(GradeCategory entity);
        Task<GradeCategory?> UpdateAsync(int id, GradeCategory entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class GradeCategoryService : IGradeCategoryService
    {
        private readonly EduPlusDbContext _context;

        public GradeCategoryService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.GradeCategories.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "weight" => sortDesc ? query.OrderByDescending(x => x.Weight) : query.OrderBy(x => x.Weight),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Weight,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<GradeCategory> CreateAsync(GradeCategory entity)
        {
            entity.IsActive = true;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.GradeCategories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<GradeCategory?> UpdateAsync(int id, GradeCategory entity)
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
                if (!await _context.GradeCategories.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.GradeCategories.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.GradeCategories.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.GradeCategories.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
