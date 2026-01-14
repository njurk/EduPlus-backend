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
        Task<IEnumerable<LessonDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? subjectId = null);
        Task<Lesson?> GetByIdAsync(int id);
        Task<Lesson> CreateAsync(CreateLessonDto dto);
        Task<Lesson?> UpdateAsync(int id, UpdateLessonDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> GenerateLessonsAsync(int classId, int schoolYearId, int semesterId);
    }

    public class LessonService : ILessonService
    {
        private readonly EduPlusDbContext _context;

        public LessonService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LessonDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? subjectId = null)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .Where(l => l.IsActive)
                .Include(l => l.Subject)
                .Include(l => l.Teacher)
                .Include(l => l.Class)
                .Include(l => l.Classroom)
                .Include(l => l.LessonHour)
                .Include(l => l.Status)
                .AsQueryable();

            if (classId.HasValue)
                query = query.Where(l => l.ClassId == classId.Value);

            if (subjectId.HasValue)
                query = query.Where(l => l.SubjectId == subjectId.Value);

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
                StartTime = l.LessonHour.StartTime.ToString(@"hh\:mm"),
                EndTime = l.LessonHour.EndTime.ToString(@"hh\:mm"),
                Topic = l.Topic,
                StatusId = l.StatusId,
                StatusName = l.Status != null ? l.Status.Name : string.Empty,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            });

            projected = sortBy?.ToLower() switch
            {
                "date" => sortDesc ? projected.OrderByDescending(l => l.Date) : projected.OrderBy(l => l.Date),
                "subject" => sortDesc ? projected.OrderByDescending(l => l.SubjectName) : projected.OrderBy(l => l.SubjectName),
                "class" => sortDesc ? projected.OrderByDescending(l => l.ClassName) : projected.OrderBy(l => l.ClassName),
                "ordernumber" => sortDesc ? projected.OrderByDescending(l => l.OrderNumber) : projected.OrderBy(l => l.OrderNumber),
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
            if (dto.StatusId.HasValue) item.StatusId = dto.StatusId.Value;
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

        public async Task<int> GenerateLessonsAsync(int classId, int schoolYearId, int semesterId)
        {
            var semester = await _context.Semesters
                .FirstOrDefaultAsync(s => s.Id == semesterId && s.SchoolYearId == schoolYearId);

            if (semester == null) return 0;

            var scheduleEntries = await _context.WeeklySchedules
                .Where(ws => ws.ClassId == classId && ws.SemesterId == semesterId && ws.IsActive)
                .ToListAsync();

            if (!scheduleEntries.Any()) return 0;

            var plannedStatus = await _context.LessonStatuses
                .FirstOrDefaultAsync(ls => ls.Name.ToLower().Contains("zaplanowana") || ls.Name.ToLower().Contains("planned"));
            
            var defaultStatusId = plannedStatus?.Id ?? 1;

            var semesterStart = semester.StartDate.ToDateTime(TimeOnly.MinValue);
            var semesterEnd = semester.EndDate.ToDateTime(TimeOnly.MinValue);
            var today = DateTime.Now.Date;

            var startDate = semesterStart;
            var isCurrentSemester = today >= semesterStart && today <= semesterEnd;
            
            if (isCurrentSemester)
            {
                startDate = today.AddDays(1);
            }

            var generatedCount = 0;
            var currentDate = startDate;

            while (currentDate <= semesterEnd)
            {
                var dayOfWeek = (int)currentDate.DayOfWeek;
                
                var daySchedules = scheduleEntries.Where(s => s.DayOfWeek == dayOfWeek).ToList();

                foreach (var schedule in daySchedules)
                {
                    var existingLesson = await _context.Lessons
                        .AnyAsync(l => l.ClassId == classId 
                            && l.Date == currentDate 
                            && l.LessonHourId == schedule.LessonHourId
                            && l.IsActive);

                    if (!existingLesson)
                    {
                        var lesson = new Lesson
                        {
                            SubjectId = schedule.SubjectId,
                            TeacherId = schedule.TeacherId,
                            ClassId = classId,
                            ClassroomId = schedule.ClassroomId,
                            LessonHourId = schedule.LessonHourId,
                            Topic = string.Empty,
                            StatusId = defaultStatusId,
                            Date = currentDate,
                            IsActive = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        _context.Lessons.Add(lesson);
                        generatedCount++;
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            await _context.SaveChangesAsync();
            return generatedCount;
        }
    }
}

