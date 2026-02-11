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
        Task<List<LessonDto>> GetScheduleForTeacherAsync(int teacherId, int? semesterId = null);
        Task<List<ScheduleTemplateDto>> GetAvailableForDateAsync(DateTime date, int? classId = null, int? teacherId = null, int? semesterId = null);
        Task<LessonDto> CreateOrUpdateAsync(WeeklyScheduleDto dto);
        Task<bool> DeleteAsync(int id);
        Task<int> ClearScheduleAsync(int classId, int semesterId);
        Task<List<object>> GetAvailableClassroomsAsync(int semesterId, int dayOfWeek, int lessonHourId, int? excludeId = null);
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
                SubjectName = ws.Subject != null ? ws.Subject.Name + (ws.Subject.IsActive ? "" : " (nieaktywny)") : "-",
                ClassId = ws.ClassId,
                ClassName = ws.Class != null ? $"{ws.Class.Level}{ws.Class.Letter}" + (ws.Class.IsActive ? "" : " (nieaktywna)") : "",
                TeacherId = ws.TeacherId,
                TeacherName = ws.Teacher != null ? $"{ws.Teacher.FirstName} {ws.Teacher.LastName}" + (ws.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                ClassroomId = ws.ClassroomId,
                ClassroomName = ws.Classroom != null ? ws.Classroom.Name + (ws.Classroom.IsActive ? "" : " (nieaktywna)") : "",
                DayOfWeek = ws.DayOfWeek,
                OrderNumber = ws.LessonHour?.OrderNumber ?? 0,
                StartTime = ws.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                EndTime = ws.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
            }).ToList();

            return result;
        }

        public async Task<List<LessonDto>> GetScheduleForTeacherAsync(int teacherId, int? semesterId = null)
        {
            var query = _context.WeeklySchedules
                .Include(ws => ws.Subject)
                .Include(ws => ws.Class)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .Where(ws => ws.TeacherId == teacherId && ws.IsActive);

            if (semesterId.HasValue)
                query = query.Where(ws => ws.SemesterId == semesterId.Value);

            var scheduleEntries = await query.ToListAsync();

            return scheduleEntries.Select(ws => new LessonDto
            {
                Id = ws.Id,
                SubjectId = ws.SubjectId,
                SubjectName = ws.Subject != null ? ws.Subject.Name + (ws.Subject.IsActive ? "" : " (nieaktywny)") : "-",
                ClassId = ws.ClassId,
                ClassName = ws.Class != null ? $"{ws.Class.Level}{ws.Class.Letter}" + (ws.Class.IsActive ? "" : " (nieaktywna)") : "",
                TeacherId = ws.TeacherId,
                TeacherName = ws.Teacher != null ? $"{ws.Teacher.FirstName} {ws.Teacher.LastName}" + (ws.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                ClassroomId = ws.ClassroomId,
                ClassroomName = ws.Classroom != null ? ws.Classroom.Name + (ws.Classroom.IsActive ? "" : " (nieaktywna)") : "",
                DayOfWeek = ws.DayOfWeek,
                OrderNumber = ws.LessonHour?.OrderNumber ?? 0,
                StartTime = ws.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                EndTime = ws.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
            }).ToList();
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
                    SubjectId = ws.SubjectId,
                    SubjectName = ws.Subject != null ? ws.Subject.Name + (ws.Subject.IsActive ? "" : " (nieaktywny)") : "-",
                    ClassName = ws.Class != null ? $"{ws.Class.Level}{ws.Class.Letter}" + (ws.Class.IsActive ? "" : " (nieaktywna)") : "",
                    TeacherName = ws.Teacher != null ? $"{ws.Teacher.FirstName} {ws.Teacher.LastName}" + (ws.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                    ClassroomName = ws.Classroom != null ? ws.Classroom.Name + (ws.Classroom.IsActive ? "" : " (nieaktywna)") : "",
                    OrderNumber = ws.LessonHour?.OrderNumber ?? 0,
                    StartTime = ws.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                    EndTime = ws.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
                })
                .OrderBy(s => s.OrderNumber)
                .ToList();

            return result;
        }

        public async Task<LessonDto> CreateOrUpdateAsync(WeeklyScheduleDto dto)
        {
            Data.Data.Entities.WeeklySchedule entity;

            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                entity = await _context.WeeklySchedules.FindAsync(dto.Id.Value) 
                    ?? throw new Exception("Nie znaleziono planu");

                var classroomConflict = await _context.WeeklySchedules.AnyAsync(ws =>
                    ws.ClassroomId == dto.ClassroomId &&
                    ws.DayOfWeek == entity.DayOfWeek &&
                    ws.LessonHourId == entity.LessonHourId &&
                    ws.SemesterId == entity.SemesterId &&
                    ws.Id != entity.Id &&
                    ws.IsActive);
                if (classroomConflict)
                    throw new InvalidOperationException("Sala jest już zajęta w tym terminie");

                var teacherConflict = await _context.WeeklySchedules.AnyAsync(ws =>
                    ws.TeacherId == dto.TeacherId &&
                    ws.DayOfWeek == entity.DayOfWeek &&
                    ws.LessonHourId == entity.LessonHourId &&
                    ws.SemesterId == entity.SemesterId &&
                    ws.Id != entity.Id &&
                    ws.IsActive);
                if (teacherConflict)
                    throw new InvalidOperationException("Nauczyciel ma już lekcję w tym terminie");

                entity.SubjectId = dto.SubjectId;
                entity.TeacherId = dto.TeacherId;
                entity.ClassroomId = dto.ClassroomId;
                entity.UpdatedAt = DateTime.Now;
            }
            else
            {
                var semester = await _context.Semesters.FindAsync(dto.SemesterId)
                    ?? throw new Exception("Nie znaleziono semestru");

                entity = new Data.Data.Entities.WeeklySchedule
                {
                    ClassId = dto.ClassId,
                    SemesterId = dto.SemesterId,
                    SchoolYearId = semester.SchoolYearId,
                    SubjectId = dto.SubjectId,
                    TeacherId = dto.TeacherId,
                    ClassroomId = dto.ClassroomId,
                    DayOfWeek = dto.DayOfWeek,
                    LessonHourId = dto.LessonHourId,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var classroomConflict = await _context.WeeklySchedules.AnyAsync(ws =>
                    ws.ClassroomId == dto.ClassroomId &&
                    ws.DayOfWeek == dto.DayOfWeek &&
                    ws.LessonHourId == dto.LessonHourId &&
                    ws.SemesterId == dto.SemesterId &&
                    ws.IsActive);
                if (classroomConflict)
                    throw new InvalidOperationException("Sala jest już zajęta w tym terminie");

                var teacherConflict = await _context.WeeklySchedules.AnyAsync(ws =>
                    ws.TeacherId == dto.TeacherId &&
                    ws.DayOfWeek == dto.DayOfWeek &&
                    ws.LessonHourId == dto.LessonHourId &&
                    ws.SemesterId == dto.SemesterId &&
                    ws.IsActive);
                if (teacherConflict)
                    throw new InvalidOperationException("Nauczyciel ma już lekcję w tym terminie");

                _context.WeeklySchedules.Add(entity);
            }

            await _context.SaveChangesAsync();

            await _context.Entry(entity).Reference(e => e.Subject).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Teacher).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Classroom).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Class).LoadAsync();
            await _context.Entry(entity).Reference(e => e.LessonHour).LoadAsync();

            return new LessonDto
            {
                Id = entity.Id,
                SubjectId = entity.SubjectId,
                SubjectName = entity.Subject != null ? entity.Subject.Name + (entity.Subject.IsActive ? "" : " (nieaktywny)") : "-",
                ClassId = entity.ClassId,
                ClassName = entity.Class != null ? $"{entity.Class.Level}{entity.Class.Letter}" + (entity.Class.IsActive ? "" : " (nieaktywna)") : "",
                TeacherId = entity.TeacherId,
                TeacherName = entity.Teacher != null ? $"{entity.Teacher.FirstName} {entity.Teacher.LastName}" + (entity.Teacher.IsActive ? "" : " (nieaktywny)") : "",
                ClassroomId = entity.ClassroomId,
                ClassroomName = entity.Classroom != null ? entity.Classroom.Name + (entity.Classroom.IsActive ? "" : " (nieaktywna)") : "",
                DayOfWeek = entity.DayOfWeek,
                OrderNumber = entity.LessonHour?.OrderNumber ?? 0,
                StartTime = entity.LessonHour?.StartTime.ToString(@"HH\:mm") ?? "",
                EndTime = entity.LessonHour?.EndTime.ToString(@"HH\:mm") ?? ""
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.WeeklySchedules.FindAsync(id);
            if (entity == null) return false;

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> ClearScheduleAsync(int classId, int semesterId)
        {
            var entities = await _context.WeeklySchedules
                .Where(ws => ws.ClassId == classId && ws.SemesterId == semesterId && ws.IsActive)
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return entities.Count;
        }

        public async Task<List<object>> GetAvailableClassroomsAsync(int semesterId, int dayOfWeek, int lessonHourId, int? excludeId = null)
        {
            var occupiedIds = await _context.WeeklySchedules
                .Where(ws => ws.SemesterId == semesterId && ws.DayOfWeek == dayOfWeek && ws.LessonHourId == lessonHourId && ws.IsActive && (!excludeId.HasValue || ws.Id != excludeId.Value))
                .Select(ws => ws.ClassroomId)
                .ToListAsync();

            return await _context.Classrooms
                .Where(c => c.IsActive && !occupiedIds.Contains(c.Id))
                .OrderBy(c => c.Name)
                .Select(c => (object)new { c.Id, c.Name })
                .ToListAsync();
        }
    }
}
