using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAnnouncementService
    {
        Task<PaginatedResponse<object>> GetAllAsync(int userId, int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? authorName = null, int? targetRoleId = null);
        Task<List<string>> GetAuthorsAsync();
        Task<Announcement?> GetByIdAsync(int id);
        Task<Announcement> CreateAsync(Announcement entity, List<int>? roleIds = null);
        Task<Announcement?> UpdateAsync(int id, string title, string description, List<int>? roleIds = null);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task MarkAsReadAsync(int announcementId, int userId);
    }

    public class AnnouncementService : IAnnouncementService
    {
        private readonly EduPlusDbContext _context;

        public AnnouncementService(EduPlusDbContext context)
        {
            _context = context;
        }


        public async Task<PaginatedResponse<object>> GetAllAsync(int userId, int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? authorName = null, int? targetRoleId = null)
        {
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var query = _context.Announcements
                .Include(a => a.Author)
                .Include(a => a.AnnouncementTargets).ThenInclude(at => at.Role)
                .AsNoTracking()
                .Where(a => showInactive ? !a.IsActive : a.IsActive);

            query = query.Where(a =>
                a.AnnouncementTargets.Any(at => at.RoleId == null) ||
                a.AnnouncementTargets.Any(at => at.RoleId != null && userRoleIds.Contains(at.RoleId.Value))
            );

            if (targetRoleId.HasValue)
            {
                if (targetRoleId.Value == 0)
                {
                    query = query.Where(a => a.AnnouncementTargets.Any(at => at.RoleId == null));
                }
                else
                {
                    query = query.Where(a => a.AnnouncementTargets.Any(at => at.RoleId == targetRoleId.Value));
                }
            }

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

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.Title,
                    a.Description,
                    a.AuthorId,
                    AuthorName = a.Author != null ? a.Author.FirstName + " " + a.Author.LastName : null,
                    TargetRoles = a.AnnouncementTargets.Any(at => at.RoleId == null)
                        ? "Wszyscy"
                        : string.Join(", ", a.AnnouncementTargets.Where(at => at.RoleId != null).Select(at => at.Role!.Name)),
                    IsRead = _context.AnnouncementReads.Any(ar => ar.AnnouncementId == a.Id && ar.UserId == userId),
                    a.IsActive,
                    a.CreatedAt,
                    a.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == a.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();

            return new PaginatedResponse<object>
            {
                Data = data.Cast<object>().ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<string>> GetAuthorsAsync()
        {
            return await _context.Announcements
                .AsNoTracking()
                .Include(a => a.Author)
                .Where(a => a.Author != null)
                .Select(a => a.Author.FirstName + " " + a.Author.LastName)
                .Distinct()
                .OrderBy(name => name)
                .ToListAsync();
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .Include(a => a.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Announcement> CreateAsync(Announcement entity, List<int>? roleIds = null)
        {
            entity.IsActive = true;
            _context.Announcements.Add(entity);
            await _context.SaveChangesAsync();

            if (roleIds == null || !roleIds.Any())
            {
                _context.AnnouncementTargets.Add(new AnnouncementTarget
                {
                    AnnouncementId = entity.Id,
                    RoleId = null
                });
            }
            else
            {
                foreach (var roleId in roleIds)
                {
                    _context.AnnouncementTargets.Add(new AnnouncementTarget
                    {
                        AnnouncementId = entity.Id,
                        RoleId = roleId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Announcement?> UpdateAsync(int id, string title, string description, List<int>? roleIds = null)
        {
            var item = await _context.Announcements
                .Include(a => a.AnnouncementTargets)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (item == null) return null;

            item.Title = title;
            item.Description = description;

            _context.AnnouncementTargets.RemoveRange(item.AnnouncementTargets);

            if (roleIds == null || !roleIds.Any())
            {
                item.AnnouncementTargets.Add(new AnnouncementTarget
                {
                    AnnouncementId = id,
                    RoleId = null
                });
            }
            else
            {
                foreach (var roleId in roleIds)
                {
                    item.AnnouncementTargets.Add(new AnnouncementTarget
                    {
                        AnnouncementId = id,
                        RoleId = roleId
                    });
                }
            }

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

        public async Task MarkAsReadAsync(int announcementId, int userId)
        {
            var exists = await _context.AnnouncementReads
                .AnyAsync(ar => ar.AnnouncementId == announcementId && ar.UserId == userId);

            if (!exists)
            {
                _context.AnnouncementReads.Add(new AnnouncementRead
                {
                    AnnouncementId = announcementId,
                    UserId = userId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}

