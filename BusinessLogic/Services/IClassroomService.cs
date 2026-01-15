using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IClassroomService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<Classroom> CreateAsync(Classroom entity);
        Task<Classroom?> UpdateAsync(int id, Classroom entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class ClassroomService : IClassroomService
    {
        private readonly EduPlusDbContext _context;

        public ClassroomService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.Classrooms.AsNoTracking().AsQueryable();

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
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<Classroom> CreateAsync(Classroom entity)
        {
            entity.IsActive = true;
            _context.Classrooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Classroom?> UpdateAsync(int id, Classroom entity)
        {
            if (id != entity.Id) return null;

            if (entity.IsActive && entity.Name.EndsWith(" (nieaktywny)"))
                entity.Name = entity.Name.Replace(" (nieaktywny)", "");

            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Classrooms.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Classrooms.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                if (!item.Name.EndsWith(" (nieaktywny)"))
                    item.Name += " (nieaktywny)";
            }
            else
            {
                _context.Classrooms.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Classrooms.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            if (item.Name.EndsWith(" (nieaktywny)"))
                item.Name = item.Name.Replace(" (nieaktywny)", "").Trim();

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

