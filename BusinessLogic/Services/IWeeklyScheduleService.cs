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
                StartTime = ws.LessonHour?.StartTime.ToString(@"hh\:mm") ?? "",
                EndTime = ws.LessonHour?.EndTime.ToString(@"hh\:mm") ?? ""
            }).ToList();

            return result;
        }
    }
}
