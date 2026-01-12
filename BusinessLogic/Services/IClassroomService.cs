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
        Task<IEnumerable<Classroom>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive);
        Task<Classroom> CreateAsync(Classroom entity);
        Task<Classroom?> UpdateAsync(int id, Classroom entity);
        Task<bool> DeleteAsync(int id);
    }

    public class ClassroomService : IClassroomService
    {
        private readonly SchoolDbContext _context;

        public ClassroomService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Classroom>> GetAllAsync(string? search, string? sortBy, bool sortDesc, bool showInactive)
        {
            var query = _context.Classrooms.AsNoTracking().AsQueryable();

            if (showInactive)
                query = query.Where(r => !r.IsActive);
            else
                query = query.Where(r => r.IsActive);

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

            return await query.ToListAsync();
        }

        public async Task<Classroom> CreateAsync(Classroom entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.IsActive = true;

            _context.Classrooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Classroom?> UpdateAsync(int id, Classroom entity)
        {
            if (id != entity.Id) return null;

            if (entity.IsActive && entity.Name.EndsWith(" (nieaktywny)"))
            {
                entity.Name = entity.Name.Replace(" (nieaktywny)", "");
            }

            entity.UpdatedAt = DateTime.Now;

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
                {
                    item.Name += " (nieaktywny)";
                }
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.Classrooms.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
