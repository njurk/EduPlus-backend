using Shared.DTOs;
using Data.Data;
using Data.Data.EntitiesForView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly EduPlusDbContext _context;

        public DashboardController(EduPlusDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
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

            var announcements = await _context.Announcements
                .AsNoTracking()
                .Include(a => a.Author)
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .Select(a => new DashboardAnnouncementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Date = a.CreatedAt.ToString("yyyy-MM-dd"),
                    Author = a.Author != null ? $"{a.Author.FirstName} {a.Author.LastName}" : "Brak danych"
                })
                .ToListAsync();

            var result = new DashboardSummaryDto
            {
                Stats = new DashboardStatsDto
                {
                    TotalUsers = statsView.TotalUsers,
                    TotalStudents = statsView.TotalStudents,
                    TotalTeachers = statsView.TotalTeachers,
                    TotalClasses = statsView.TotalClasses
                },
                Status = new DashboardStatusDto
                {
                    SchoolYear = currentSemester?.SchoolYear?.Name ?? "Brak danych",
                    Semester = currentSemester?.Name ?? "-"
                },
                Announcements = announcements
            };

            return Ok(result);
        }

        [HttpGet("attendance-chart")]
        public async Task<ActionResult<IEnumerable<AttendanceChartDto>>> GetAttendanceChart()
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

            return Ok(result);
        }
    }
}
