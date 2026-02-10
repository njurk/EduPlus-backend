using Data.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IBadgeService
    {
        Task<UnreadCountsDto> GetUnreadCountsAsync(int userId);
    }

    public class BadgeService : IBadgeService
    {
        private readonly EduPlusDbContext _context;

        public BadgeService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<UnreadCountsDto> GetUnreadCountsAsync(int userId)
        {
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var unreadAnnouncementIds = await _context.Announcements
                .Where(a => a.IsActive)
                .Where(a =>
                    a.AnnouncementTargets.Any(at => at.RoleId == null) ||
                    a.AnnouncementTargets.Any(at => at.RoleId != null && userRoleIds.Contains(at.RoleId.Value)))
                .Where(a => !_context.AnnouncementReads.Any(ar => ar.AnnouncementId == a.Id && ar.UserId == userId))
                .Select(a => a.Id)
                .ToListAsync();

            var unreadTicketIds = await _context.Tickets
                .Where(t => !t.IsClosed)
                .Where(t => !_context.TicketReads.Any(tr => tr.TicketId == t.Id && tr.UserId == userId))
                .Select(t => t.Id)
                .ToListAsync();

            var pendingExcusesCount = 0;
            var homeroomClassIds = await _context.Classes
                .Where(c => c.HomeroomTeacherId == userId && c.IsActive)
                .Select(c => c.Id)
                .ToListAsync();

            if (homeroomClassIds.Count > 0)
            {
                var homeroomStudentIds = await _context.ClassStudents
                    .Where(cs => homeroomClassIds.Contains(cs.ClassId) && cs.IsActive)
                    .Select(cs => cs.StudentId)
                    .ToListAsync();

                pendingExcusesCount = await _context.Excuses
                    .Where(e => e.IsActive && e.IsAccepted == null && homeroomStudentIds.Contains(e.StudentId))
                    .CountAsync();
            }

            return new UnreadCountsDto
            {
                Announcements = unreadAnnouncementIds.Count,
                Tickets = unreadTicketIds.Count,
                Excuses = pendingExcusesCount,
                UnreadAnnouncementIds = unreadAnnouncementIds,
                UnreadTicketIds = unreadTicketIds
            };
        }
    }
}
