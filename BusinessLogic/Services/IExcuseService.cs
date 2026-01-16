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
    public interface IExcuseService
    {
        Task<IEnumerable<ExcuseDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool? isAccepted = null);
        Task<Excuse?> GetByIdAsync(int id);
        Task<Excuse> CreateAsync(CreateExcuseDto dto);
        Task<Excuse?> UpdateAsync(int id, UpdateExcuseDto dto);
        Task<bool> AcceptAsync(int id, bool isAccepted);
        Task<bool> DeleteAsync(int id);
    }

    public class ExcuseService : IExcuseService
    {
        private readonly EduPlusDbContext _context;

        public ExcuseService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExcuseDto>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = true, bool? isAccepted = null)
        {
            var query = _context.Excuses
                .AsNoTracking()
                .Where(e => e.IsActive)
                .Include(e => e.Attendance)
                    .ThenInclude(a => a.Student)
                .Include(e => e.Attendance)
                    .ThenInclude(a => a.Lesson)
                        .ThenInclude(l => l.Subject)
                .Include(e => e.Parent)
                .AsQueryable();

            if (isAccepted.HasValue)
                query = query.Where(e => e.IsAccepted == isAccepted.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(e => e.Reason.ToLower().Contains(searchLower) 
                    || e.Attendance.Student.LastName.ToLower().Contains(searchLower)
                    || e.Parent.LastName.ToLower().Contains(searchLower));
            }

            var projected = query.Select(e => new ExcuseDto
            {
                Id = e.Id,
                AttendanceId = e.AttendanceId,
                StudentName = e.Attendance.Student.LastName + " " + e.Attendance.Student.FirstName,
                SubjectName = e.Attendance.Lesson.Subject.Name,
                LessonDate = e.Attendance.Lesson.Date,
                ParentId = e.ParentId,
                ParentName = e.Parent.LastName + " " + e.Parent.FirstName,
                Reason = e.Reason,
                SubmittedAt = e.SubmittedAt,
                IsAccepted = e.IsAccepted,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            });

            projected = sortBy?.ToLower() switch
            {
                "student" => sortDesc ? projected.OrderByDescending(e => e.StudentName) : projected.OrderBy(e => e.StudentName),
                "parent" => sortDesc ? projected.OrderByDescending(e => e.ParentName) : projected.OrderBy(e => e.ParentName),
                "isaccepted" => sortDesc ? projected.OrderByDescending(e => e.IsAccepted) : projected.OrderBy(e => e.IsAccepted),
                "createdat" => sortDesc ? projected.OrderByDescending(e => e.CreatedAt) : projected.OrderBy(e => e.CreatedAt),
                _ => sortDesc ? projected.OrderByDescending(e => e.SubmittedAt) : projected.OrderBy(e => e.SubmittedAt)
            };

            return await projected.ToListAsync();
        }

        public async Task<Excuse?> GetByIdAsync(int id)
        {
            return await _context.Excuses
                .Include(e => e.Attendance)
                    .ThenInclude(a => a.Student)
                .Include(e => e.Parent)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<Excuse> CreateAsync(CreateExcuseDto dto)
        {
            var entity = new Excuse
            {
                AttendanceId = dto.AttendanceId,
                ParentId = dto.ParentId,
                Reason = dto.Reason,
                SubmittedAt = DateTime.Now,
                IsAccepted = null,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Excuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Excuse?> UpdateAsync(int id, UpdateExcuseDto dto)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return null;

            if (dto.Reason != null) item.Reason = dto.Reason;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> AcceptAsync(int id, bool isAccepted)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsAccepted = isAccepted;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
