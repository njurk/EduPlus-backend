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
        Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? authorName = null);
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

        public async Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? authorName = null)
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

            if (!string.IsNullOrWhiteSpace(authorName))
            {
                query = query.Where(a => a.Author != null && (a.Author.FirstName + " " + a.Author.LastName) == authorName);
            }

            query = sortBy?.ToLower() switch
            {
                "title" => sortDesc ? query.OrderByDescending(a => a.Title) : query.OrderBy(a => a.Title),
                "author" or "authorname" => sortDesc ? query.OrderByDescending(a => a.Author!.LastName) : query.OrderBy(a => a.Author!.LastName),
                "updated" or "updatedat" => sortDesc ? query.OrderByDescending(a => a.UpdatedAt) : query.OrderBy(a => a.UpdatedAt),
                "created" or "createdat" or _ => sortDesc ? query.OrderByDescending(a => a.CreatedAt) : query.OrderBy(a => a.CreatedAt)
            };

            return await query
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    a.Description,
                    a.AuthorId,
                    AuthorName = a.Author != null ? a.Author.FirstName + " " + a.Author.LastName : null,
                    a.IsActive,
                    a.CreatedAt,
                    a.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == a.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
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

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Announcements.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                if (!item.Title.EndsWith(" (nieaktywny)"))
                    item.Title = $"{item.Title} (nieaktywny)";
                item.IsActive = false;
            }
            else
            {
                _context.Announcements.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Announcements.IgnoreQueryFilters().FirstOrDefaultAsync(a => a.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            if (item.Title.EndsWith(" (nieaktywny)"))
                item.Title = item.Title.Replace(" (nieaktywny)", "").Trim();

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

