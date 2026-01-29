using Data.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface ILayoutService
    {
        Task<UnreadCountsDto> GetUnreadCountsAsync(int userId);
    }

    public class LayoutService : ILayoutService
    {
        private readonly EduPlusDbContext _context;

        public LayoutService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<UnreadCountsDto> GetUnreadCountsAsync(int userId)
        {
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var unreadAnnouncements = await _context.Announcements
                .Where(a => a.IsActive)
                .Where(a =>
                    a.AnnouncementTargets.Any(at => at.RoleId == null) ||
                    a.AnnouncementTargets.Any(at => at.RoleId != null && userRoleIds.Contains(at.RoleId.Value)))
                .Where(a => !_context.AnnouncementReads.Any(ar => ar.AnnouncementId == a.Id && ar.UserId == userId))
                .CountAsync();

            var unreadTickets = await _context.Tickets
                .Where(t => !t.IsClosed)
                .Where(t => !_context.TicketReads.Any(tr => tr.TicketId == t.Id && tr.UserId == userId))
                .CountAsync();

            return new UnreadCountsDto
            {
                Announcements = unreadAnnouncements,
                Tickets = unreadTickets
            };
        }
    }
}
