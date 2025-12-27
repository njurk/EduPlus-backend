using Data.Data;
using Data.Data.EntitiesForView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public DashboardController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("teacher/{teacherId}/classes")]
        public async Task<ActionResult<IEnumerable<ViewTeacherClass>>> GetTeacherClasses(int teacherId)
        {
            return await _context.ViewTeacherClasses
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync();
        }

        [HttpGet("student/{studentId}/grades")]
        public async Task<ActionResult<IEnumerable<ViewGradeDetails>>> GetStudentGrades(int studentId, [FromQuery] int? semesterId)
        {
            var query = _context.ViewGradeDetails.Where(x => x.StudentId == studentId);
        
        if (semesterId.HasValue)
                query = query.Where(x => x.SemesterId == semesterId.Value);

        return await query.ToListAsync();
        }

        [HttpGet("student/{studentId}/attendance")]
        public async Task<ActionResult<IEnumerable<ViewAttendanceDetails>>> GetStudentAttendance(int studentId)
        {
            return await _context.ViewAttendanceDetails
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        [HttpGet("student/{studentId}/attendance-stats")]
        public async Task<ActionResult<ViewStudentAttendance>> GetStudentAttendanceStats(int studentId)
        {
            return await _context.ViewStudentAttendances
                .FirstOrDefaultAsync(x => x.StudentId == studentId)
                ?? (ActionResult<ViewStudentAttendance>)NotFound();
        }

        [HttpGet("class/{classId}/schedule")]
        public async Task<ActionResult<IEnumerable<ViewLessonSchedule>>> GetClassSchedule(int classId)
        {
            return await _context.ViewLessonSchedules
                .Where(x => x.ClassId == classId)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ToListAsync();
        }

        [HttpGet("upcoming-events")]
        public async Task<ActionResult<IEnumerable<ViewUpcomingEvent>>> GetUpcomingEvents([FromQuery] int? classId)
        {
            var query = _context.ViewUpcomingEvents.AsQueryable();
        
        if (classId.HasValue)
                query = query.Where(x => x.ClassId == classId || x.ClassId == null);

        return await query
            .Where(x => x.StartDateTime >= DateTime.UtcNow)
            .OrderBy(x => x.StartDateTime)
            .Take(5)
            .ToListAsync();
        }

        [HttpGet("announcements")]
        public async Task<ActionResult<IEnumerable<ViewAnnouncementDetails>>> GetAnnouncements()
        {
            return await _context.ViewAnnouncementDetails
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        [HttpGet("student/{studentId}/behavior-summary")]
        public async Task<ActionResult<ViewBehaviorGradeSummary>> GetBehaviorSummary(int studentId, [FromQuery] int semesterId)
        {
            return await _context.ViewBehaviorGradeSummaries
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.SemesterId == semesterId)
                ?? (ActionResult<ViewBehaviorGradeSummary>)NotFound();
        }

        [HttpGet("teacher/{teacherId}/pending-excuses")]
        public async Task<ActionResult<IEnumerable<ViewPendingExcuse>>> GetPendingExcuses(int teacherId)
        {
            return await _context.ViewPendingExcuses
                .Where(x => x.TeacherId == teacherId)
                .OrderBy(x => x.SubmittedAt)
                .ToListAsync();
        }
    }
}
