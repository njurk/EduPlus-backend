using Data.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Mobile;

namespace BusinessLogic.Services
{
    public interface IMobileService
    {
        Task<MobileScheduleDto?> GetScheduleAsync(int userId);
        Task<MobileGradesDto> GetGradesAsync(int userId, int? semesterId = null);
        Task<MobileAttendanceDto> GetAttendanceAsync(int userId, int? semesterId = null);
        Task<List<MobileAnnouncementDto>> GetAnnouncementsAsync(int userId);
    }

    public class MobileService : IMobileService
    {
        private readonly EduPlusDbContext _context;

        public MobileService(EduPlusDbContext context)
        {
            _context = context;
        }

        private async Task<int> GetStudentIdAsync(int userId)
        {
            var parentStudent = await _context.ParentStudents
                .Where(ps => ps.ParentId == userId)
                .FirstOrDefaultAsync();
            
            return parentStudent?.StudentId ?? userId;
        }

        public async Task<MobileScheduleDto?> GetScheduleAsync(int userId)
        {
            var studentId = await GetStudentIdAsync(userId);
            
            var classStudent = await _context.ClassStudents
                .Include(cs => cs.Class)
                .Where(cs => cs.StudentId == studentId && cs.IsActive)
                .FirstOrDefaultAsync();

            if (classStudent == null) return null;

            var currentSemester = await _context.Semesters
                .Where(s => s.StartDate <= DateOnly.FromDateTime(DateTime.Now) && s.EndDate >= DateOnly.FromDateTime(DateTime.Now))
                .FirstOrDefaultAsync();

            if (currentSemester == null) return null;

            var lessons = await _context.WeeklySchedules
                .Include(ws => ws.Subject)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .Where(ws => ws.ClassId == classStudent.ClassId && ws.SemesterId == currentSemester.Id && ws.IsActive)
                .Select(ws => new MobileLessonDto
                {
                    DayOfWeek = ws.DayOfWeek,
                    OrderNumber = ws.LessonHour != null ? ws.LessonHour.OrderNumber : 0,
                    StartTime = ws.LessonHour != null ? ws.LessonHour.StartTime.ToString(@"HH\:mm") : "",
                    EndTime = ws.LessonHour != null ? ws.LessonHour.EndTime.ToString(@"HH\:mm") : "",
                    SubjectName = ws.Subject != null ? ws.Subject.Name : "",
                    TeacherName = ws.Teacher != null ? ws.Teacher.FirstName + " " + ws.Teacher.LastName : "",
                    ClassroomName = ws.Classroom != null ? ws.Classroom.Name : ""
                })
                .OrderBy(l => l.DayOfWeek).ThenBy(l => l.OrderNumber)
                .ToListAsync();

            return new MobileScheduleDto
            {
                ClassId = classStudent.ClassId,
                ClassName = classStudent.Class != null ? $"{classStudent.Class.Level}{classStudent.Class.Letter}" : "",
                SemesterId = currentSemester.Id,
                SemesterName = currentSemester.Name,
                Lessons = lessons
            };
        }

        public async Task<MobileGradesDto> GetGradesAsync(int userId, int? semesterId = null)
        {
            var studentId = await GetStudentIdAsync(userId);
            
            var query = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .Where(g => g.StudentId == studentId && g.IsActive);

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    var start = semester.StartDate.ToDateTime(TimeOnly.MinValue);
                    var end = semester.EndDate.ToDateTime(TimeOnly.MaxValue);
                    query = query.Where(g => g.CreatedAt >= start && g.CreatedAt <= end);
                }
            }

            var grades = await query.ToListAsync();

            var subjects = grades
                .GroupBy(g => new { g.SubjectId, SubjectName = g.Subject?.Name ?? "" })
                .Select(group => new MobileSubjectGradesDto
                {
                    SubjectId = group.Key.SubjectId,
                    SubjectName = group.Key.SubjectName,
                    Average = (double?)group.Where(g => g.GradeType?.Value != null).Average(g => g.GradeType!.Value),
                    Grades = group.OrderByDescending(g => g.CreatedAt).Select(g => new MobileGradeDto
                    {
                        Value = g.GradeType?.Numeric ?? "",
                        CategoryName = g.GradeCategory?.Name ?? "",
                        CategoryColorHex = g.GradeCategory?.ColorHex ?? "",
                        TeacherName = g.Teacher != null ? $"{g.Teacher.FirstName} {g.Teacher.LastName}" : "",
                        Comment = g.Comment,
                        CreatedAt = g.CreatedAt
                    }).ToList()
                })
                .OrderBy(s => s.SubjectName)
                .ToList();

            return new MobileGradesDto { Subjects = subjects };
        }

        public async Task<MobileAttendanceDto> GetAttendanceAsync(int userId, int? semesterId = null)
        {
            var studentId = await GetStudentIdAsync(userId);
            
            var query = _context.Attendances
                .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                .Include(a => a.AttendanceType)
                .Where(a => a.StudentId == studentId && a.IsActive);

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    query = query.Where(a => DateOnly.FromDateTime(a.Lesson.Date) >= semester.StartDate && DateOnly.FromDateTime(a.Lesson.Date) <= semester.EndDate);
                }
            }

            var attendances = await query.ToListAsync();

            var subjects = attendances
                .GroupBy(a => a.Lesson?.Subject?.Name ?? "")
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .Select(group =>
                {
                    var total = group.Count();
                    var present = group.Count(a => a.AttendanceType?.ShortCode == "OB");
                    var absent = group.Count(a => a.AttendanceType?.ShortCode == "NB");
                    var late = group.Count(a => a.AttendanceType?.ShortCode == "SP");
                    var excused = group.Count(a => a.AttendanceType?.ShortCode == "US");

                    return new MobileSubjectAttendanceDto
                    {
                        SubjectName = group.Key,
                        TotalLessons = total,
                        Present = present,
                        Absent = absent,
                        Late = late,
                        Excused = excused,
                        AttendancePercentage = total > 0 ? Math.Round((double)(present + late + excused) / total * 100, 1) : 0
                    };
                })
                .OrderBy(s => s.SubjectName)
                .ToList();

            return new MobileAttendanceDto { Subjects = subjects };
        }

        public async Task<List<MobileAnnouncementDto>> GetAnnouncementsAsync(int userId)
        {
            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var readIds = await _context.AnnouncementReads
                .Where(ar => ar.UserId == userId)
                .Select(ar => ar.AnnouncementId)
                .ToListAsync();

            var announcements = await _context.Announcements
                .Include(a => a.AnnouncementTargets)
                .Where(a => a.IsActive &&
                    (a.AnnouncementTargets.Any(at => at.RoleId == null) ||
                     a.AnnouncementTargets.Any(at => at.RoleId != null && userRoleIds.Contains(at.RoleId.Value))))
                .OrderByDescending(a => a.CreatedAt)
                .Take(50)
                .Select(a => new MobileAnnouncementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Description,
                    CreatedAt = a.CreatedAt,
                    IsRead = readIds.Contains(a.Id)
                })
                .ToListAsync();

            return announcements;
        }
    }
}
