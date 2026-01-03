using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Data.Data;
using API.DTOs;
using Data.Data.EntitiesForView;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public DashboardController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary()
        {
            var statsView = await _context.DashboardStats.FirstOrDefaultAsync();

            if (statsView == null)
            {
                statsView = new DashboardStats();
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            var currentSemester = await _context.Semesters
                .Include(s => s.SchoolYear)
                .Where(s => s.IsActive)
                .Where(s => s.StartDate <= today && s.EndDate >= today)
                .FirstOrDefaultAsync();

            if (currentSemester == null)
            {
                currentSemester = await _context.Semesters
                    .Include(s => s.SchoolYear)
                    .Where(s => s.IsActive && s.StartDate <= today)
                    .OrderByDescending(s => s.StartDate)
                    .FirstOrDefaultAsync();
            }

            var announcements = await _context.Announcements
                .Include(a => a.Author)
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .Select(a => new DashboardAnnouncementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Date = a.CreatedAt.ToString("yyyy-MM-dd"),
                    Author = a.Author != null ? $"{a.Author.FirstName} {a.Author.LastName}" : "System"
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
                    Semester = currentSemester?.Name ?? "-",
                    AvgGrade = statsView.AvgGradeGlobal.ToString("0.00")
                },

                Announcements = announcements
            };

            return Ok(result);
        }

        [HttpGet("attendance-chart")]
        public async Task<ActionResult<IEnumerable<AttendanceChartDto>>> GetAttendanceChart()
        {
            var todayDateTime = DateTime.UtcNow.Date;
            var todayDateOnly = DateOnly.FromDateTime(todayDateTime);
            var sevenDaysAgo = todayDateOnly.AddDays(-6);

            var presentTypeIds = await _context.AttendanceTypes
                .Where(at => at.ShortCode == "OB" || at.ShortCode == "SP")
                .Select(at => at.Id)
                .ToListAsync();

            var attendanceData = await _context.Attendances
                .Include(a => a.Lesson)
                .Where(a => a.IsActive && a.Lesson != null &&
                            a.Lesson.Date >= sevenDaysAgo.ToDateTime(TimeOnly.MinValue) &&
                            a.Lesson.Date <= todayDateOnly.ToDateTime(TimeOnly.MinValue))
                .Select(a => new
                {
                    Date = a.Lesson.Date,
                    IsPresent = presentTypeIds.Contains(a.AttendanceTypeId)
                })
                .ToListAsync();

            var result = new List<AttendanceChartDto>();
            var culture = new System.Globalization.CultureInfo("pl-PL");

            for (int i = 0; i < 7; i++)
            {
                var currentDay = sevenDaysAgo.AddDays(i);
                var currentDayDateTime = currentDay.ToDateTime(TimeOnly.MinValue);
                var dayData = attendanceData.Where(a => a.Date.Date == currentDayDateTime.Date).ToList();
                int percentage = 0;
                if (dayData.Any())
                {
                    double presentCount = dayData.Count(a => a.IsPresent);
                    percentage = (int)Math.Round((presentCount / dayData.Count) * 100);
                }

                result.Add(new AttendanceChartDto
                {
                    Date = currentDay.ToString("dd.MM"),
                    DayName = culture.DateTimeFormat.GetAbbreviatedDayName(currentDayDateTime.DayOfWeek),
                    AttendancePercentage = percentage
                });
            }

            return Ok(result);
        }
    }
}