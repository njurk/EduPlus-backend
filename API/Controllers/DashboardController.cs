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

            var currentSemester = await _context.Semesters
                .Include(s => s.SchoolYear)
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();

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
                    Semester = currentSemester != null ? $"Semestr {currentSemester.Id}" : "-",

                    AvgAttendance = $"{Math.Round(statsView.AvgAttendanceToday, 1)}%",
                    AvgGrade = statsView.AvgGradeGlobal.ToString("0.00")
                },

                Announcements = announcements
            };

            return Ok(result);
        }
    }
}