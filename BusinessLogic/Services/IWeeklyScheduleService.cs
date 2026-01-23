using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Data;

namespace BusinessLogic.Services
{
    public interface IWeeklyScheduleService
    {
        Task<List<LessonDto>> GetScheduleForClassAsync(int classId, int? semesterId = null);
        Task<List<ScheduleTemplateDto>> GetAvailableForDateAsync(DateTime date, int? classId = null, int? teacherId = null, int? semesterId = null);
    }

    public class WeeklyScheduleService : IWeeklyScheduleService
    {
        private readonly EduPlusDbContext _context;

        public WeeklyScheduleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<List<LessonDto>> GetScheduleForClassAsync(int classId, int? semesterId = null)
        {
            var query = _context.WeeklySchedules
                .Include(ws => ws.Subject)
                .Include(ws => ws.Class)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .Where(ws => ws.ClassId == classId && ws.IsActive);

            if (semesterId.HasValue)
                query = query.Where(ws => ws.SemesterId == semesterId.Value);

            var scheduleEntries = await query.ToListAsync();

            var result = scheduleEntries.Select(ws => new LessonDto
            {
                Id = ws.Id,
                SubjectId = ws.SubjectId,
                SubjectName = ws.Subject?.Name ?? "-",
                ClassId = ws.ClassId,
                ClassName = ws.Class != null ? $"{ws.Class.Level}{ws.Class.Letter}" : "",
                TeacherId = ws.TeacherId,
                TeacherName = ws.Teacher != null ? $"{ws.Teacher.FirstName} {ws.Teacher.LastName}" : "",
                ClassroomId = ws.ClassroomId,
                ClassroomName = ws.Classroom?.Name ?? "",
                DayOfWeek = ws.DayOfWeek,
                OrderNumber = ws.LessonHour?.OrderNumber ?? 0,
                StartTime = ws.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                EndTime = ws.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
            }).ToList();

            return result;
        }

        public async Task<List<ScheduleTemplateDto>> GetAvailableForDateAsync(DateTime date, int? classId = null, int? teacherId = null, int? semesterId = null)
        {
            var dayOfWeek = (int)date.DayOfWeek;

            var query = _context.WeeklySchedules
                .Include(ws => ws.Subject)
                .Include(ws => ws.Class)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .Where(ws => ws.DayOfWeek == dayOfWeek && ws.IsActive);

            if (classId.HasValue)
                query = query.Where(ws => ws.ClassId == classId.Value);

            if (teacherId.HasValue)
                query = query.Where(ws => ws.TeacherId == teacherId.Value);

            if (semesterId.HasValue)
                query = query.Where(ws => ws.SemesterId == semesterId.Value);

            var scheduleEntries = await query.ToListAsync();

            var existingLessons = await _context.Lessons
                .Where(l => l.Date == date && l.IsActive)
                .Select(l => new { l.ClassId, l.LessonHourId })
                .ToListAsync();

            var result = scheduleEntries
                .Where(ws => !existingLessons.Any(el => el.ClassId == ws.ClassId && el.LessonHourId == ws.LessonHourId))
                .Select(ws => new ScheduleTemplateDto
                {
                    Id = ws.Id,
                    SubjectName = ws.Subject?.Name ?? "-",
                    ClassName = ws.Class != null ? $"{ws.Class.Level}{ws.Class.Letter}" : "",
                    TeacherName = ws.Teacher != null ? $"{ws.Teacher.FirstName} {ws.Teacher.LastName}" : "",
                    ClassroomName = ws.Classroom?.Name ?? "",
                    OrderNumber = ws.LessonHour?.OrderNumber ?? 0,
                    StartTime = ws.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                    EndTime = ws.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
                })
                .OrderBy(s => s.OrderNumber)
                .ToList();

            return result;
        }
    }
}
