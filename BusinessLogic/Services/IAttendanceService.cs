using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive, string? search = null, string? sortBy = null, bool sortDesc = true);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance> CreateAsync(Attendance entity);
        Task<Attendance?> UpdateAsync(int id, int attendanceTypeId);
        Task<bool> RestoreAsync(int id);
        Task<bool> DeleteAsync(int id);
    }

    public class AttendanceService : IAttendanceService
    {
        private readonly EduPlusDbContext _context;

        public AttendanceService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive, string? search = null, string? sortBy = null, bool sortDesc = true)
        {
            var query = _context.Attendances.AsNoTracking()
                .Include(a => a.Student)
                .Include(a => a.AttendanceType)
                .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                .Include(a => a.Lesson).ThenInclude(l => l.Teacher)
                .Include(a => a.Lesson).ThenInclude(l => l.LessonHour)
                .AsQueryable();

            if (!includeInactive) query = query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(a =>
                    a.Student.LastName.ToLower().Contains(s) ||
                    a.Student.FirstName.ToLower().Contains(s) ||
                    a.Lesson.Subject.Name.ToLower().Contains(s) ||
                    a.Lesson.Teacher.LastName.ToLower().Contains(s));
            }

            var projected = query.Select(a => new AttendanceAdminDto
                {
                    Id = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.LastName + " " + a.Student.FirstName,
                    StudentEmail = a.Student.Email,
                    SubjectName = a.Lesson.Subject.Name,
                    TeacherName = a.Lesson.Teacher.LastName + " " + a.Lesson.Teacher.FirstName,
                    LessonDate = a.Lesson.Date,
                    OrderNumber = a.Lesson.LessonHour.OrderNumber,
                    TypeName = a.AttendanceType.Name,
                    ShortCode = a.AttendanceType.ShortCode,
                    AttendanceTypeId = a.AttendanceTypeId,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsActive = a.IsActive
                });

            projected = sortBy?.ToLower() switch
            {
                "updatedat" => sortDesc ? projected.OrderByDescending(a => a.UpdatedAt) : projected.OrderBy(a => a.UpdatedAt),
                "typename" or "status" => sortDesc ? projected.OrderByDescending(a => a.TypeName) : projected.OrderBy(a => a.TypeName),
                "studentname" => sortDesc ? projected.OrderByDescending(a => a.StudentName) : projected.OrderBy(a => a.StudentName),
                "subjectname" => sortDesc ? projected.OrderByDescending(a => a.SubjectName) : projected.OrderBy(a => a.SubjectName),
                "createdat" => sortDesc ? projected.OrderByDescending(a => a.CreatedAt) : projected.OrderBy(a => a.CreatedAt),
                _ => sortDesc ? projected.OrderByDescending(a => a.LessonDate) : projected.OrderBy(a => a.LessonDate)
            };

            return await projected.ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Attendance> CreateAsync(Attendance entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.IsActive = true;

            _context.Attendances.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Attendance?> UpdateAsync(int id, int attendanceTypeId)
        {
            var item = await _context.Attendances.FindAsync(id);
            if (item == null) return null;

            item.AttendanceTypeId = attendanceTypeId;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Attendances.FindAsync(id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Attendances.FindAsync(id);
            if (item == null) return false;

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

