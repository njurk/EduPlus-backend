using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? subjectId = null, int? semesterId = null, int? schoolYearId = null, bool showInactive = false);
        Task<Lesson?> GetByIdAsync(int id);
        Task<LessonDetailsDto?> GetDetailsAsync(int id);
        Task<IEnumerable<LessonAttendanceDto>> GetLessonAttendanceAsync(int lessonId);
        Task<bool> UpdateLessonAttendanceAsync(int lessonId, int studentId, int? attendanceTypeId);
        Task<Lesson> CreateAsync(CreateLessonDto dto);
        Task<Lesson?> UpdateAsync(int id, UpdateLessonDto dto);
        Task<bool> DeleteAsync(int id);
        Task<Lesson> CreateFromScheduleAsync(int scheduleId, DateTime date, int? teacherIdOverride = null);
    }

    public class LessonService : ILessonService
    {
        private readonly EduPlusDbContext _context;

        public LessonService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LessonDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? subjectId = null, int? semesterId = null, int? schoolYearId = null, bool showInactive = false)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .Where(l => showInactive ? !l.IsActive : l.IsActive)
                .Include(l => l.Subject)
                .Include(l => l.Teacher)
                .Include(l => l.Class)
                .Include(l => l.Classroom)
                .Include(l => l.LessonHour)
                .Include(l => l.Status)
                .AsQueryable();

            var users = _context.Users.AsNoTracking();

            if (classId.HasValue)
                query = query.Where(l => l.ClassId == classId.Value);

            if (subjectId.HasValue)
                query = query.Where(l => l.SubjectId == subjectId.Value);

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    var startDate = semester.StartDate.ToDateTime(TimeOnly.MinValue);
                    var endDate = semester.EndDate.ToDateTime(TimeOnly.MaxValue);
                    query = query.Where(l => l.Date >= startDate && l.Date <= endDate);
                }
            }
            else if (schoolYearId.HasValue)
            {
                var schoolYear = await _context.SchoolYears.FindAsync(schoolYearId.Value);
                if (schoolYear != null)
                {
                    var startDate = schoolYear.StartDate.ToDateTime(TimeOnly.MinValue);
                    var endDate = schoolYear.EndDate.ToDateTime(TimeOnly.MaxValue);
                    query = query.Where(l => l.Date >= startDate && l.Date <= endDate);
                }
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(l => l.Topic.ToLower().Contains(searchLower) || l.Subject.Name.ToLower().Contains(searchLower));
            }

            var projected = query.Select(l => new LessonDto
            {
                Id = l.Id,
                SubjectId = l.SubjectId,
                SubjectName = l.Subject.Name,
                ClassId = l.ClassId,
                ClassName = l.Class.Level + l.Class.Letter,
                TeacherId = l.TeacherId,
                TeacherName = l.Teacher.LastName + " " + l.Teacher.FirstName,
                ClassroomId = l.ClassroomId,
                ClassroomName = l.Classroom != null ? l.Classroom.Name : string.Empty,
                Date = l.Date,
                OrderNumber = l.LessonHour.OrderNumber,
                StartTime = l.LessonHour.StartTime.ToString(@"HH\:mm"),
                EndTime = l.LessonHour.EndTime.ToString(@"HH\:mm"),
                Topic = l.Topic,
                StatusId = l.StatusId,
                StatusName = l.Status != null ? l.Status.Name : string.Empty,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                ModifiedByName = l.ModifiedByUserId != null ? users.Where(u => u.Id == l.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() : null
            });

            projected = sortBy?.ToLower() switch
            {
                "date" => sortDesc ? projected.OrderByDescending(l => l.Date) : projected.OrderBy(l => l.Date),
                "subject" => sortDesc ? projected.OrderByDescending(l => l.SubjectName) : projected.OrderBy(l => l.SubjectName),
                "class" => sortDesc ? projected.OrderByDescending(l => l.ClassName) : projected.OrderBy(l => l.ClassName),
                "ordernumber" => sortDesc ? projected.OrderByDescending(l => l.OrderNumber) : projected.OrderBy(l => l.OrderNumber),
                "created" => sortDesc ? projected.OrderByDescending(l => l.CreatedAt) : projected.OrderBy(l => l.CreatedAt),
                "updated" => sortDesc ? projected.OrderByDescending(l => l.UpdatedAt) : projected.OrderBy(l => l.UpdatedAt),
                _ => sortDesc ? projected.OrderByDescending(l => l.Date).ThenByDescending(l => l.OrderNumber) : projected.OrderBy(l => l.Date).ThenBy(l => l.OrderNumber)
            };

            return await projected.ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons
                .Include(l => l.Subject)
                .Include(l => l.Teacher)
                .Include(l => l.Class)
                .Include(l => l.Classroom)
                .Include(l => l.LessonHour)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lesson> CreateAsync(CreateLessonDto dto)
        {
            var entity = new Lesson
            {
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                ClassId = dto.ClassId,
                ClassroomId = dto.ClassroomId,
                LessonHourId = dto.LessonHourId,
                Topic = dto.Topic ?? string.Empty,
                StatusId = dto.StatusId,
                Date = dto.Date,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Lessons.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Lesson?> UpdateAsync(int id, UpdateLessonDto dto)
        {
            var item = await _context.Lessons.FindAsync(id);
            if (item == null) return null;

            if (dto.Topic != null) item.Topic = dto.Topic;
            if (dto.StatusId.HasValue)
            {
                item.StatusId = dto.StatusId.Value;
                
                var cancelledStatus = await _context.LessonStatuses
                    .FirstOrDefaultAsync(ls => ls.Slug == "cancelled");
                
                if (cancelledStatus != null && dto.StatusId.Value == cancelledStatus.Id)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC sp_DeactivateLessonAttendance @LessonId = {0}", id);
                }
            }
            if (dto.ClassroomId.HasValue) item.ClassroomId = dto.ClassroomId.Value;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Lessons.FindAsync(id);
            if (item == null) return false;

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Lesson> CreateFromScheduleAsync(int scheduleId, DateTime date, int? teacherIdOverride = null)
        {
            var schedule = await _context.WeeklySchedules
                .Include(ws => ws.Subject)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Class)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .FirstOrDefaultAsync(ws => ws.Id == scheduleId && ws.IsActive);

            if (schedule == null)
                throw new ArgumentException("Plan nie istnieje lub jest nieaktywny");

            var scheduleDayOfWeek = schedule.DayOfWeek;
            var dateDayOfWeek = (int)date.DayOfWeek;
            if (scheduleDayOfWeek != dateDayOfWeek)
                throw new ArgumentException("Dzień tygodnia nie pasuje do aktualnego planu");

            var existingLesson = await _context.Lessons
                .AnyAsync(l => l.ClassId == schedule.ClassId 
                    && l.Date == date 
                    && l.LessonHourId == schedule.LessonHourId
                    && l.IsActive);

            if (existingLesson)
                throw new InvalidOperationException("Lekcja w tym terminie już istnieje");

            var defaultStatus = await _context.LessonStatuses
                .FirstOrDefaultAsync(ls => ls.Slug == "completed");
            
            var lesson = new Lesson
            {
                SubjectId = schedule.SubjectId,
                TeacherId = teacherIdOverride ?? schedule.TeacherId,
                ClassId = schedule.ClassId,
                ClassroomId = schedule.ClassroomId,
                LessonHourId = schedule.LessonHourId,
                Topic = string.Empty,
                StatusId = defaultStatus?.Id ?? 1,
                Date = date,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_GenerateLessonAttendance @LessonId = {0}, @ClassId = {1}", 
                lesson.Id, lesson.ClassId);
            
            return lesson;
        }

        public async Task<LessonDetailsDto?> GetDetailsAsync(int id)
        {
            var users = _context.Users.AsNoTracking();

            var lesson = await _context.Lessons
                .AsNoTracking()
                .Where(l => l.Id == id)
                .Include(l => l.Subject)
                .Include(l => l.Teacher)
                .Include(l => l.Class)
                .Include(l => l.Classroom)
                .Include(l => l.LessonHour)
                .Include(l => l.Status)
                .Select(l => new LessonDetailsDto
                {
                    Id = l.Id,
                    SubjectId = l.SubjectId,
                    SubjectName = l.Subject.Name,
                    ClassId = l.ClassId,
                    ClassName = l.Class.Level + l.Class.Letter,
                    TeacherId = l.TeacherId,
                    TeacherName = l.Teacher.LastName + " " + l.Teacher.FirstName,
                    ClassroomId = l.ClassroomId,
                    ClassroomName = l.Classroom != null ? l.Classroom.Name : string.Empty,
                    Date = l.Date,
                    DayOfWeek = (int)l.Date.DayOfWeek,
                    OrderNumber = l.LessonHour.OrderNumber,
                    StartTime = l.LessonHour.StartTime.ToString(@"HH\:mm"),
                    EndTime = l.LessonHour.EndTime.ToString(@"HH\:mm"),
                    Topic = l.Topic,
                    StatusId = l.StatusId,
                    StatusName = l.Status != null ? l.Status.Name : string.Empty,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt,
                    ModifiedByName = l.ModifiedByUserId != null ? users.Where(u => u.Id == l.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() : null
                })
                .FirstOrDefaultAsync();

            return lesson;
        }

        public async Task<IEnumerable<LessonAttendanceDto>> GetLessonAttendanceAsync(int lessonId)
        {
            var lesson = await _context.Lessons
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == lessonId);

            if (lesson == null)
                return Enumerable.Empty<LessonAttendanceDto>();

            var classStudents = await _context.ClassStudents
                .AsNoTracking()
                .Where(cs => cs.ClassId == lesson.ClassId)
                .Include(cs => cs.Student)
                .OrderBy(cs => cs.Student.LastName)
                .ThenBy(cs => cs.Student.FirstName)
                .Select(cs => new { cs.StudentId, cs.Student.LastName, cs.Student.FirstName })
                .ToListAsync();

            var attendances = await _context.Attendances
                .AsNoTracking()
                .Where(a => a.LessonId == lessonId && a.IsActive)
                .Include(a => a.AttendanceType)
                .ToDictionaryAsync(a => a.StudentId, a => a);

            var result = classStudents.Select((cs, index) =>
            {
                attendances.TryGetValue(cs.StudentId, out var attendance);
                return new LessonAttendanceDto
                {
                    Id = attendance?.Id ?? 0,
                    StudentId = cs.StudentId,
                    StudentName = cs.LastName + " " + cs.FirstName,
                    StudentNumber = index + 1,
                    AttendanceTypeId = attendance?.AttendanceTypeId,
                    AttendanceTypeName = attendance?.AttendanceType?.Name ?? string.Empty,
                    ShortCode = attendance?.AttendanceType?.ShortCode ?? string.Empty,
                    ColorHex = attendance?.AttendanceType?.ColorHex ?? string.Empty
                };
            });

            return result;
        }

        public async Task<bool> UpdateLessonAttendanceAsync(int lessonId, int studentId, int? attendanceTypeId)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) return false;

            var existing = await _context.Attendances
                .FirstOrDefaultAsync(a => a.LessonId == lessonId && a.StudentId == studentId);

            if (attendanceTypeId == null)
            {
                if (existing != null)
                {
                    existing.IsActive = false;
                    existing.UpdatedAt = DateTime.Now;
                }
            }
            else
            {
                if (existing != null)
                {
                    existing.AttendanceTypeId = attendanceTypeId.Value;
                    existing.IsActive = true;
                    existing.UpdatedAt = DateTime.Now;
                }
                else
                {
                    var newAttendance = new Attendance
                    {
                        LessonId = lessonId,
                        StudentId = studentId,
                        AttendanceTypeId = attendanceTypeId.Value,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.Attendances.Add(newAttendance);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

