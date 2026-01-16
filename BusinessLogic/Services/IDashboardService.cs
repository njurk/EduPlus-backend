using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.EntitiesForView;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<AttendanceChartDto>> GetAttendanceChartAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly EduPlusDbContext _context;

        public DashboardService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var statsView = await _context.DashboardStats.FirstOrDefaultAsync() ?? new DashboardStatsView();

            var today = DateOnly.FromDateTime(DateTime.Now);

            var currentSemester = await _context.Semesters
                .AsNoTracking()
                .Include(s => s.SchoolYear)
                .Where(s => s.IsActive)
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();

            if (currentSemester == null)
            {
                currentSemester = await _context.Semesters
                    .AsNoTracking()
                    .Include(s => s.SchoolYear)
                    .Where(s => s.IsActive && s.StartDate <= today)
                    .OrderByDescending(s => s.StartDate)
                    .FirstOrDefaultAsync();
            }

            var parentRoleId = await _context.Roles
                .Where(r => r.Name.ToLower().Contains("rodzic") || r.Name.ToLower().Contains("parent"))
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var totalParents = parentRoleId > 0
                ? await _context.UserRoles.CountAsync(ur => ur.RoleId == parentRoleId && ur.User.IsActive)
                : 0;

            var recentTickets = await _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => !t.IsClosed)
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .Select(t => new DashboardTicketDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    UserName = t.User != null ? $"{t.User.FirstName} {t.User.LastName}" : "Nieznany",
                    CreatedAt = t.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    IsClosed = t.IsClosed
                })
                .ToListAsync();

            return new DashboardSummaryDto
            {
                Stats = new DashboardStatsDto
                {
                    TotalUsers = statsView.TotalUsers,
                    TotalStudents = statsView.TotalStudents,
                    TotalTeachers = statsView.TotalTeachers,
                    TotalClasses = statsView.TotalClasses,
                    TotalParents = totalParents
                },
                Status = new DashboardStatusDto
                {
                    SchoolYear = currentSemester?.SchoolYear?.Name ?? "Brak danych",
                    Semester = currentSemester?.Name ?? "-"
                },
                RecentTickets = recentTickets
            };
        }

        public async Task<IEnumerable<AttendanceChartDto>> GetAttendanceChartAsync()
        {
            var dbData = await _context.Database
                .SqlQueryRaw<WeeklyAttendanceResultDto>("EXEC sp_GetWeeklyAttendance")
                .ToListAsync();

            var result = new List<AttendanceChartDto>();
            var culture = new CultureInfo("pl-PL");
            var today = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                var loopDate = today.AddDays(-i);
                var dayStat = dbData.FirstOrDefault(d => d.Date.Date == loopDate);

                result.Add(new AttendanceChartDto
                {
                    Date = loopDate.ToString("dd.MM"),
                    DayName = culture.DateTimeFormat.GetAbbreviatedDayName(loopDate.DayOfWeek),
                    AttendancePercentage = dayStat?.Percentage ?? 0
                });
            }

            return result;
        }
    }
}
