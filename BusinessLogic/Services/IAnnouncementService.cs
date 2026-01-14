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
    public interface IAnnouncementService
    {
        Task<IEnumerable<Announcement>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false);
        Task<Announcement?> GetByIdAsync(int id);
        Task<Announcement> CreateAsync(Announcement entity);
        Task<Announcement?> UpdateAsync(int id, string title, string description);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class AnnouncementService : IAnnouncementService
    {
        private readonly EduPlusDbContext _context;

        public AnnouncementService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false)
        {
            var query = _context.Announcements
                .Include(a => a.Author)
                .AsNoTracking()
                .Where(a => showInactive ? !a.IsActive : a.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(a => a.Title.ToLower().Contains(searchLower) || a.Description.ToLower().Contains(searchLower));
            }

            query = sortBy?.ToLower() switch
            {
                "title" => sortDesc ? query.OrderByDescending(a => a.Title) : query.OrderBy(a => a.Title),
                "author" => sortDesc ? query.OrderByDescending(a => a.Author.LastName) : query.OrderBy(a => a.Author.LastName),
                "updatedat" => sortDesc ? query.OrderByDescending(a => a.UpdatedAt) : query.OrderBy(a => a.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .Include(a => a.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Announcement> CreateAsync(Announcement entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.IsActive = true;

            _context.Announcements.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Announcement?> UpdateAsync(int id, string title, string description)
        {
            var item = await _context.Announcements.FindAsync(id);
            if (item == null) return null;

            item.Title = title;
            item.Description = description;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Announcements.FindAsync(id);
            if (item == null) return false;

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Announcements.FindAsync(id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

