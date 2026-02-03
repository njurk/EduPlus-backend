using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IGradeTypeService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<GradeType> CreateAsync(GradeType entity);
        Task<GradeType?> UpdateAsync(int id, GradeType entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class GradeTypeService : IGradeTypeService
    {
        private readonly EduPlusDbContext _context;

        public GradeTypeService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.GradeTypes.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "value" => sortDesc ? query.OrderByDescending(x => x.Value) : query.OrderBy(x => x.Value),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.Value) : query.OrderBy(x => x.Value)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Numeric,
                    x.Value,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<GradeType> CreateAsync(GradeType entity)
        {
            if (entity.Value < 0)
                throw new InvalidOperationException("Wartość oceny nie może być ujemna");

            if (await _context.GradeTypes.AnyAsync(x => x.Numeric == entity.Numeric && x.IsActive))
                throw new InvalidOperationException("Taki symbol oceny już istnieje");

            entity.IsActive = true;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.GradeTypes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<GradeType?> UpdateAsync(int id, GradeType entity)
        {
            if (id != entity.Id) return null;

            if (entity.Value < 0)
                throw new InvalidOperationException("Wartość oceny nie może być ujemna");

            if (await _context.GradeTypes.AnyAsync(x => x.Numeric == entity.Numeric && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Taki symbol oceny już istnieje");

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
                if (!await _context.GradeTypes.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.GradeTypes.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.GradeTypes.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.GradeTypes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
