using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Data.Data;
using API.DTOs;

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

        // GET: api/dashboard/stats
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            var stats = new DashboardStatsDto
            {
                TotalUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalStudents = await _context.UserRoles.CountAsync(ur => ur.RoleId == 4 && ur.IsActive),
                TotalTeachers = await _context.UserRoles.CountAsync(ur => ur.RoleId == 2 && ur.IsActive),
                TotalClasses = await _context.Classes.CountAsync(c => c.IsActive)
            };

            return Ok(stats);
        }

        // GET: api/dashboard/status
        [HttpGet("status")]
        public async Task<ActionResult<DashboardStatusDto>> GetSystemStatus()
        {
            var today = DateTime.UtcNow.Date;
            var currentSemester = await _context.Semesters
                .Include(s => s.SchoolYear)
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();

            var attendanceQuery = _context.Attendances
                .Include(a => a.Lesson)
                .Where(a => a.IsActive && a.Lesson.Date.Date == today);

            var totalAttendanceEntries = await attendanceQuery.CountAsync();
            var presentEntries = await attendanceQuery.CountAsync(a => a.AttendanceTypeId == 1);

            double attendancePct = 0;
            if (totalAttendanceEntries > 0)
            {
                attendancePct = (double)presentEntries / totalAttendanceEntries * 100;
            }

            var gradesQuery = _context.Grades
                .Where(g => g.IsActive);

            var avgGrade = 0.0;
            if (await gradesQuery.AnyAsync())
            {
                avgGrade = await gradesQuery.AverageAsync(g => g.GradeTypeId);
            }

            return Ok(new DashboardStatusDto
            {
                SchoolYear = currentSemester?.SchoolYear?.Name ?? "Brak danych",
                Semester = currentSemester != null ? $"Semestr {currentSemester.Id}" : "-",
                AvgAttendanceToday = $"{Math.Round(attendancePct, 1)}%",
                AvgGradeThisSemester = Math.Round(avgGrade, 2).ToString("0.00")
            });
        }

        // GET: api/dashboard/announcements
        [HttpGet("announcements")]
        public async Task<ActionResult<IEnumerable<DashboardAnnouncementDto>>> GetRecentAnnouncements()
        {
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

            return Ok(announcements);
        }
    }
}