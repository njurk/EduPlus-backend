using Data.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class WeeklyScheduleService : IWeeklyScheduleService
    {
        private readonly EduPlusDbContext _context;

        public WeeklyScheduleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<List<LessonDto>> GetScheduleForClassAsync(int classId, DateTime dateFrom, DateTime dateTo)
        {
            var lessons = await _context.Lessons
                .Include(l => l.Subject)
                .Include(l => l.Class)
                .Include(l => l.Teacher)
                .Include(l => l.Classroom)
                .Include(l => l.LessonHour)
                .Where(l => l.ClassId == classId && l.Date >= dateFrom && l.Date <= dateTo && l.IsActive)
                .ToListAsync();

            var result = lessons.Select(l => new LessonDto
            {
                Id = l.Id,
                SubjectId = l.SubjectId,
                SubjectName = l.Subject?.Name ?? "Nieznany",
                ClassId = l.ClassId,
                ClassName = l.Class != null ? $"{l.Class.Level}{l.Class.Letter}" : "",
                TeacherId = l.TeacherId,
                TeacherName = l.Teacher != null ? $"{l.Teacher.FirstName} {l.Teacher.LastName}" : "",
                ClassroomId = l.ClassroomId,
                ClassroomName = l.Classroom?.Name ?? "",
                Date = l.Date,
                OrderNumber = l.LessonHour?.OrderNumber ?? 0,
                StartTime = l.LessonHour?.StartTime.ToString(@"hh\:mm") ?? "",
                EndTime = l.LessonHour?.EndTime.ToString(@"hh\:mm") ?? "",
                Topic = l.Topic
            }).ToList();

            return result;
        }
    }
}
