using Data.Data.EntitiesForView;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Data.Data;

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

        [HttpGet("schedule/class/{classId}")]
        public async Task<ActionResult<IEnumerable<ViewLessonSchedule>>> GetClassSchedule(int classId)
        {
            return await _context.ViewLessonSchedules
                .Where(x => x.ClassId == classId)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ToListAsync();
        }

        [HttpGet("grades/student/{studentId}")]
        public async Task<ActionResult<IEnumerable<ViewGradeDetails>>> GetStudentGrades(int studentId)
        {
            return await _context.ViewGradeDetails
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        [HttpGet("attendance/student/{studentId}")]
        public async Task<ActionResult<IEnumerable<ViewAttendanceDetails>>> GetStudentAttendance(int studentId)
        {
            return await _context.ViewAttendanceDetails
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        [HttpGet("attendance-summary/student/{studentId}")]
        public async Task<ActionResult<ViewStudentAttendance>> GetStudentAttendanceSummary(int studentId)
        {
            var summary = await _context.ViewStudentAttendances
                .FirstOrDefaultAsync(x => x.StudentId == studentId);

            if (summary == null) return NotFound();
            return Ok(summary);
        }

        [HttpGet("behavior/student/{studentId}")]
        public async Task<ActionResult<IEnumerable<ViewBehaviorDetails>>> GetStudentBehaviorNotes(int studentId)
        {
            return await _context.ViewBehaviorDetails
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        [HttpGet("upcoming-events")]
        public async Task<ActionResult<IEnumerable<ViewUpcomingEvent>>> GetUpcomingEvents([FromQuery] int? classId)
        {
            var query = _context.ViewUpcomingEvents.AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(x => x.ClassId == classId || x.ClassId == null);
            }

            return await query
                .Where(x => x.StartDateTime >= DateTime.UtcNow)
                .OrderBy(x => x.StartDateTime)
                .ToListAsync();
        }

        [HttpGet("announcements")]
        public async Task<ActionResult<IEnumerable<ViewAnnouncementDetails>>> GetAnnouncements()
        {
            return await _context.ViewAnnouncementDetails
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}