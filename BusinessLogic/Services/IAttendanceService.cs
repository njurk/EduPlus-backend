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
        Task<IEnumerable<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance> CreateAsync(Attendance entity);
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

        public async Task<IEnumerable<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive)
        {
            var query = _context.Attendances.AsNoTracking();

            if (!includeInactive) query = query.Where(x => x.IsActive);

            return await query
                .Include(a => a.Student)
                .Include(a => a.AttendanceType)
                .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                .Include(a => a.Lesson).ThenInclude(l => l.Teacher)
                .Select(a => new AttendanceAdminDto
                {
                    Id = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.LastName + " " + a.Student.FirstName,
                    StudentEmail = a.Student.Email,
                    SubjectName = a.Lesson.Subject.Name,
                    TeacherName = a.Lesson.Teacher.LastName + " " + a.Lesson.Teacher.FirstName,
                    LessonDate = a.Lesson.Date,
                    TypeName = a.AttendanceType.Name,
                    ShortCode = a.AttendanceType.ShortCode,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsActive = a.IsActive
                })
                .OrderByDescending(a => a.LessonDate)
                .ToListAsync();
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
