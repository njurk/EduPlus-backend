using Data.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Mobile;

namespace BusinessLogic.Services
{
    public interface IMobileService
    {
        Task<MobileScheduleDto?> GetScheduleAsync(int userId, int? studentId = null);
        Task<MobileGradesDto> GetGradesAsync(int userId, int? studentId = null, int? semesterId = null);
        Task<MobileAttendanceDto> GetAttendanceAsync(int userId, int? studentId = null, int? semesterId = null, DateOnly? date = null);
        Task<List<MobileAnnouncementDto>> GetAnnouncementsAsync(int userId);
        Task<List<MobileNegativeAttendanceDto>> GetNegativeAttendancesAsync(int userId, int? studentId = null, int? semesterId = null);
        Task CreateExcuseAsync(int userId, int? studentId, CreateMobileExcuseDto dto);
        Task<List<MobileExcuseDto>> GetExcusesAsync(int userId, int? studentId = null, int? semesterId = null);
        Task<List<MobileSemesterDto>> GetSemestersAsync();
        Task<List<MobileChildDto>> GetChildrenAsync(int userId);
    }


    public class MobileService : IMobileService
    {
        private readonly EduPlusDbContext _context;

        public MobileService(EduPlusDbContext context)
        {
            _context = context;
        }

        private async Task<int> GetStudentIdAsync(int userId, int? studentId = null)
        {
            if (studentId.HasValue) return studentId.Value;

            var parentStudent = await _context.ParentStudents
                .Where(ps => ps.ParentId == userId)
                .FirstOrDefaultAsync();
            
            return parentStudent?.StudentId ?? userId;
        }

        public async Task<MobileScheduleDto?> GetScheduleAsync(int userId, int? studentId = null)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);
            
            var classStudent = await _context.ClassStudents
                .Include(cs => cs.Class)
                .Where(cs => cs.StudentId == resolvedStudentId && cs.IsActive)
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
                    SubjectName = ws.Subject != null ? ws.Subject.Name + (ws.Subject.IsActive ? "" : " (nieaktywny)") : "",
                    TeacherName = ws.Teacher != null ? ws.Teacher.FirstName + " " + ws.Teacher.LastName + (ws.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                    ClassroomName = ws.Classroom != null ? ws.Classroom.Name + (ws.Classroom.IsActive ? "" : " (nieaktywna)") : ""
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

        public async Task<MobileGradesDto> GetGradesAsync(int userId, int? studentId = null, int? semesterId = null)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);
            
            var query = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .Where(g => g.StudentId == resolvedStudentId && g.IsActive);

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
                        Id = g.Id,
                        Value = g.GradeType?.Numeric ?? "",
                        CategoryName = g.GradeCategory?.Name ?? "",
                        CategoryColorHex = g.GradeCategory?.ColorHex ?? "",
                        TeacherName = g.Teacher != null ? $"{g.Teacher.FirstName} {g.Teacher.LastName}" + (g.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                        Comment = g.Comment,
                        Weight = g.GradeCategory?.Weight ?? 1,
                        CreatedAt = g.CreatedAt
                    }).ToList()
                })
                .OrderBy(s => s.SubjectName)
                .ToList();

            var recentGrades = grades
                .OrderByDescending(g => g.CreatedAt)
                .Take(5)
                .Select(g => new MobileRecentGradeDto
                {
                    Id = g.Id,
                    SubjectName = g.Subject?.Name ?? "",
                    Value = g.GradeType?.Numeric ?? "",
                    CategoryName = g.GradeCategory?.Name ?? "",
                    CategoryColorHex = g.GradeCategory?.ColorHex ?? "",
                    TeacherName = g.Teacher != null ? $"{g.Teacher.FirstName} {g.Teacher.LastName}" + (g.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                    Comment = g.Comment,
                    Weight = g.GradeCategory?.Weight ?? 1,
                    Date = g.CreatedAt.ToString("dd.MM.yyyy"),
                    CreatedAt = g.CreatedAt
                })
                .ToList();

            return new MobileGradesDto { Subjects = subjects, RecentGrades = recentGrades };
        }

        public async Task<MobileAttendanceDto> GetAttendanceAsync(int userId, int? studentId = null, int? semesterId = null, DateOnly? date = null)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);
            
            var query = _context.Attendances
                .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                .Include(a => a.AttendanceType)
                .Where(a => a.StudentId == resolvedStudentId && a.IsActive);

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

            var recentRecords = attendances
                .OrderByDescending(a => a.Lesson?.Date)
                .ThenByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new MobileAttendanceRecordDto
                {
                    SubjectName = a.Lesson?.Subject?.Name ?? "",
                    Date = a.Lesson?.Date.ToString("dd.MM.yyyy") ?? "",
                    Type = a.AttendanceType?.ShortCode ?? "",
                    TypeColorHex = a.AttendanceType?.ColorHex ?? ""
                })
                .ToList();

            var dailyLessons = new List<MobileDailyLessonDto>();
            
            if (date.HasValue)
            {
                var classStudent = await _context.ClassStudents
                    .Where(cs => cs.StudentId == studentId && cs.IsActive)
                    .FirstOrDefaultAsync();

                if (classStudent != null)
                {
                    var dayOfWeek = (int)date.Value.DayOfWeek;
                    
                    var scheduleLessons = await _context.WeeklySchedules
                        .Include(ws => ws.Subject)
                        .Include(ws => ws.LessonHour)
                        .Where(ws => ws.ClassId == classStudent.ClassId && ws.DayOfWeek == dayOfWeek && ws.IsActive)
                        .OrderBy(ws => ws.LessonHour.OrderNumber)
                        .ToListAsync();

                    var dateAttendances = attendances
                        .Where(a => a.Lesson != null && DateOnly.FromDateTime(a.Lesson.Date) == date.Value)
                        .ToDictionary(a => a.Lesson!.SubjectId);

                    dailyLessons = scheduleLessons.Select(ws => new MobileDailyLessonDto
                    {
                        LessonOrder = ws.LessonHour?.OrderNumber ?? 0,
                        StartTime = ws.LessonHour?.StartTime.ToString(@"hh\:mm") ?? "",
                        EndTime = ws.LessonHour?.EndTime.ToString(@"hh\:mm") ?? "",
                        SubjectName = ws.Subject?.Name ?? "",
                        AttendanceType = dateAttendances.TryGetValue(ws.SubjectId, out var att) ? att.AttendanceType?.ShortCode : null,
                        AttendanceTypeColorHex = dateAttendances.TryGetValue(ws.SubjectId, out var att2) ? att2.AttendanceType?.ColorHex : null
                    }).ToList();
                }
            }

            var attendanceTypes = await _context.AttendanceTypes.Where(at => at.IsActive).ToListAsync();
            
            var stats = attendanceTypes.Select(at => new MobileAttendanceStatDto
            {
                ShortCode = at.ShortCode,
                Name = at.Name,
                ColorHex = at.ColorHex,
                Count = attendances.Count(a => a.AttendanceTypeId == at.Id),
                IsNegative = at.IsNegative
            }).Where(s => s.Count > 0).ToList();

            var totalLessons = attendances.Count;

            return new MobileAttendanceDto 
            { 
                Subjects = subjects, 
                RecentRecords = recentRecords, 
                DailyLessons = dailyLessons,
                Stats = stats,
                TotalLessons = totalLessons
            };
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

            var today = DateOnly.FromDateTime(DateTime.Today);
            var currentSchoolYear = await _context.SchoolYears
                .Where(sy => sy.IsActive && sy.StartDate <= today && sy.EndDate >= today)
                .FirstOrDefaultAsync();

            var announcements = await _context.Announcements
                .Include(a => a.AnnouncementTargets)
                .Include(a => a.Author)
                .Where(a => a.IsActive &&
                    (a.AnnouncementTargets.Any(at => at.RoleId == null) ||
                     a.AnnouncementTargets.Any(at => at.RoleId != null && userRoleIds.Contains(at.RoleId.Value))) &&
                    (currentSchoolYear == null || 
                     (a.CreatedAt >= currentSchoolYear.StartDate.ToDateTime(TimeOnly.MinValue) &&
                      a.CreatedAt <= currentSchoolYear.EndDate.ToDateTime(TimeOnly.MaxValue))))
                .OrderByDescending(a => a.CreatedAt)
                .Take(50)
                .Select(a => new MobileAnnouncementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Description,
                    AuthorName = a.Author != null ? a.Author.FirstName + " " + a.Author.LastName + (a.Author.IsActive ? "" : " (nieaktywny)") : "",
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt != a.CreatedAt ? a.UpdatedAt : null,
                    IsRead = readIds.Contains(a.Id)
                })
                .ToListAsync();

            return announcements;
        }

        public async Task<List<MobileNegativeAttendanceDto>> GetNegativeAttendancesAsync(int userId, int? studentId = null, int? semesterId = null)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);

            var excusedAttendanceIds = await _context.ExcuseAttendances
                .Select(ea => ea.AttendanceId)
                .ToListAsync();

            var excusableSlugs = new[] { "absent", "late" };

            var query = _context.Attendances
                .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                .Include(a => a.Lesson).ThenInclude(l => l.LessonHour)
                .Include(a => a.AttendanceType)
                .Where(a => a.StudentId == resolvedStudentId 
                    && a.IsActive 
                    && a.AttendanceType != null
                    && excusableSlugs.Contains(a.AttendanceType.Slug)
                    && !excusedAttendanceIds.Contains(a.Id));

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    query = query.Where(a => DateOnly.FromDateTime(a.Lesson.Date) >= semester.StartDate 
                        && DateOnly.FromDateTime(a.Lesson.Date) <= semester.EndDate);
                }
            }

            var negativeAttendances = await query
                .OrderByDescending(a => a.Lesson.Date)
                .ThenByDescending(a => a.Lesson.LessonHour.OrderNumber)
                .Select(a => new MobileNegativeAttendanceDto
                {
                    Id = a.Id,
                    Date = a.Lesson.Date,
                    SubjectName = a.Lesson.Subject != null ? a.Lesson.Subject.Name : "",
                    LessonHour = a.Lesson.LessonHour != null ? a.Lesson.LessonHour.OrderNumber : 0,
                    AttendanceType = a.AttendanceType != null ? a.AttendanceType.ShortCode : "",
                    AttendanceTypeColorHex = a.AttendanceType != null ? a.AttendanceType.ColorHex : ""
                })
                .ToListAsync();

            return negativeAttendances;
        }

        public async Task CreateExcuseAsync(int userId, int? studentId, CreateMobileExcuseDto dto)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);

            var excuse = new Data.Data.Entities.Excuse
            {
                StudentId = resolvedStudentId,
                ParentId = userId,
                Reason = dto.Reason,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Excuses.Add(excuse);
            await _context.SaveChangesAsync();

            foreach (var attendanceId in dto.AttendanceIds)
            {
                _context.ExcuseAttendances.Add(new Data.Data.Entities.ExcuseAttendance
                {
                    ExcuseId = excuse.Id,
                    AttendanceId = attendanceId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<MobileExcuseDto>> GetExcusesAsync(int userId, int? studentId = null, int? semesterId = null)
        {
            var resolvedStudentId = await GetStudentIdAsync(userId, studentId);

            var query = _context.Excuses
                .Include(e => e.ExcuseAttendances)
                    .ThenInclude(ea => ea.Attendance)
                        .ThenInclude(a => a.Lesson)
                            .ThenInclude(l => l.Subject)
                .Include(e => e.ExcuseAttendances)
                    .ThenInclude(ea => ea.Attendance)
                        .ThenInclude(a => a.Lesson)
                            .ThenInclude(l => l.LessonHour)
                .Include(e => e.ExcuseAttendances)
                    .ThenInclude(ea => ea.Attendance)
                        .ThenInclude(a => a.AttendanceType)
                .Where(e => e.StudentId == resolvedStudentId && e.IsActive)
                .AsQueryable();

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    query = query.Where(e => e.ExcuseAttendances.Any(ea => 
                        DateOnly.FromDateTime(ea.Attendance.Lesson.Date) >= semester.StartDate 
                        && DateOnly.FromDateTime(ea.Attendance.Lesson.Date) <= semester.EndDate));
                }
            }

            var excuses = await query.OrderByDescending(e => e.CreatedAt).ToListAsync();

            return excuses.Select(e => new MobileExcuseDto
            {
                Id = e.Id,
                Reason = e.Reason,
                Status = e.IsAccepted == null ? "Oczekujące" : e.IsAccepted == true ? "Zaakceptowane" : "Odrzucone",
                StatusColorHex = e.IsAccepted == null ? "#f59e0b" : e.IsAccepted == true ? "#22c55e" : "#ef4444",
                CreatedAt = e.CreatedAt,
                Attendances = e.ExcuseAttendances.Select(ea => new MobileExcuseAttendanceDto
                {
                    Id = ea.AttendanceId,
                    SubjectName = ea.Attendance.Lesson?.Subject?.Name ?? "",
                    Date = ea.Attendance.Lesson?.Date.ToString("dd.MM.yyyy") ?? "",
                    LessonHour = ea.Attendance.Lesson?.LessonHour?.OrderNumber ?? 0,
                    AttendanceType = ea.Attendance.AttendanceType?.ShortCode ?? ""
                }).ToList()
            }).ToList();
        }

        public async Task<List<MobileSemesterDto>> GetSemestersAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var currentSchoolYear = await _context.SchoolYears
                .Where(sy => sy.IsActive && sy.StartDate <= today && sy.EndDate >= today)
                .FirstOrDefaultAsync();

            if (currentSchoolYear == null) return new List<MobileSemesterDto>();

            var semesters = await _context.Semesters
                .Where(s => s.SchoolYearId == currentSchoolYear.Id)
                .OrderBy(s => s.StartDate)
                .Select(s => new MobileSemesterDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsCurrent = s.StartDate <= today && s.EndDate >= today
                })
                .ToListAsync();

            return semesters;
        }

        public async Task<List<MobileChildDto>> GetChildrenAsync(int userId)
        {
            var children = await _context.ParentStudents
                .Include(ps => ps.Student)
                .Where(ps => ps.ParentId == userId)
                .Select(ps => new MobileChildDto
                {
                    Id = ps.StudentId,
                    Name = ps.Student != null ? $"{ps.Student.FirstName} {ps.Student.LastName}" + (ps.Student.IsActive ? "" : " (nieaktywny)") : "",
                    ClassName = _context.ClassStudents
                        .Include(cs => cs.Class)
                        .Where(cs => cs.StudentId == ps.StudentId && cs.IsActive)
                        .Select(cs => cs.Class != null ? $"{cs.Class.Level}{cs.Class.Letter}" : null)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return children;
        }
    }
}

