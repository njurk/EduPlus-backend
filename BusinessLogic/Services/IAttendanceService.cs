using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.API.DTOs;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAttendanceService
    {
        Task<PaginatedResponse<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive, int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, DateTime? date = null, string? subjectName = null, string? teacherName = null, string? attendanceTypeShortCode = null, int? orderNumber = null);
        Task<IEnumerable<Attendance>> GetAllAsync();
        Task<Attendance?> UpdateAsync(int id, int attendanceTypeId, IEmailService emailService);
    }

    public class AttendanceService : IAttendanceService
    {
        private readonly EduPlusDbContext _context;

        public AttendanceService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<AttendanceAdminDto>> GetAllForAdminAsync(bool includeInactive, int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, DateTime? date = null, string? subjectName = null, string? teacherName = null, string? attendanceTypeShortCode = null, int? orderNumber = null)
        {
            var query = _context.AttendanceAdminList.AsNoTracking().AsQueryable();

            query = query.Where(x => includeInactive ? !x.IsActive : x.IsActive);

            if (classId.HasValue)
                query = query.Where(a => a.ClassId == classId.Value);

            if (date.HasValue)
                query = query.Where(a => a.LessonDate.Date == date.Value.Date);

            if (!string.IsNullOrWhiteSpace(subjectName))
                query = query.Where(a => a.SubjectName == subjectName);

            if (!string.IsNullOrWhiteSpace(teacherName))
            {
                var tName = teacherName.Trim().ToLower();
                query = query.Where(a => a.TeacherName.ToLower().Contains(tName));
            }

            if (!string.IsNullOrWhiteSpace(attendanceTypeShortCode))
                query = query.Where(a => a.ShortCode == attendanceTypeShortCode);

            if (orderNumber.HasValue)
                query = query.Where(a => a.OrderNumber == orderNumber.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(a =>
                    a.StudentName.ToLower().Contains(s) ||
                    a.SubjectName.ToLower().Contains(s) ||
                    a.TeacherName.ToLower().Contains(s));
            }

            var projected = query.Select(a => new AttendanceAdminDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.StudentName,
                StudentEmail = a.StudentEmail,
                SubjectName = a.SubjectName,
                TeacherName = a.TeacherName,
                LessonDate = a.LessonDate,
                OrderNumber = a.OrderNumber,
                ClassName = a.ClassName,
                TypeName = a.TypeName,
                ShortCode = a.ShortCode,
                AttendanceTypeId = a.AttendanceTypeId,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                ModifiedByName = a.ModifiedByName,
                IsActive = a.IsActive
            });

            projected = sortBy?.ToLower() switch
            {
                "updated" or "updatedat" => sortDesc ? projected.OrderByDescending(a => a.UpdatedAt) : projected.OrderBy(a => a.UpdatedAt),
                "created" or "createdat" => sortDesc ? projected.OrderByDescending(a => a.CreatedAt) : projected.OrderBy(a => a.CreatedAt),
                "typename" or "status" => sortDesc ? projected.OrderByDescending(a => a.TypeName) : projected.OrderBy(a => a.TypeName),
                "studentname" => sortDesc ? projected.OrderByDescending(a => a.StudentName) : projected.OrderBy(a => a.StudentName),
                "subjectname" => sortDesc ? projected.OrderByDescending(a => a.SubjectName) : projected.OrderBy(a => a.SubjectName),
                "ordernumber" => sortDesc ? projected.OrderByDescending(a => a.OrderNumber) : projected.OrderBy(a => a.OrderNumber),
                "lessondate" => sortDesc ? projected.OrderByDescending(a => a.LessonDate) : projected.OrderBy(a => a.LessonDate),
                _ => sortDesc ? projected.OrderByDescending(a => a.CreatedAt) : projected.OrderBy(a => a.CreatedAt)
            };

            var totalCount = await projected.CountAsync();
            var data = await projected.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResponse<AttendanceAdminDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<Attendance?> UpdateAsync(int id, int attendanceTypeId, IEmailService emailService)
        {
            if (!await _context.AttendanceTypes.AnyAsync(at => at.Id == attendanceTypeId && at.IsActive))
                throw new InvalidOperationException("Wybrany rodzaj frekwencji nie istnieje lub jest nieaktywny");

            var item = await _context.Attendances.FindAsync(id);
            if (item == null) return null;

            var oldTypeId = item.AttendanceTypeId;
            item.AttendanceTypeId = attendanceTypeId;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var newType = await _context.AttendanceTypes.FindAsync(attendanceTypeId);
            if (newType?.IsNegative == true && oldTypeId != attendanceTypeId)
            {
                var data = await _context.Attendances
                    .Include(a => a.Student)
                    .Include(a => a.Lesson).ThenInclude(l => l.Subject)
                    .Include(a => a.Lesson).ThenInclude(l => l.Teacher)
                    .Where(a => a.Id == id)
                    .Select(a => new { StudentName = a.Student.LastName + " " + a.Student.FirstName, a.Student.Email, a.StudentId, SubjectName = a.Lesson.Subject.Name, TeacherName = a.Lesson.Teacher.LastName + " " + a.Lesson.Teacher.FirstName, a.Lesson.Date })
                    .FirstAsync();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await emailService.SendNegativeAttendanceEmailAsync(data.Email, data.StudentName, data.SubjectName, newType.Name, data.TeacherName, data.Date);
                        var parents = await _context.ParentStudents.Include(ps => ps.Parent).Where(ps => ps.StudentId == data.StudentId).Select(ps => ps.Parent!.Email).ToListAsync();
                        foreach (var email in parents)
                            await emailService.SendNegativeAttendanceEmailAsync(email, data.StudentName, data.SubjectName, newType.Name, data.TeacherName, data.Date);
                    }
                    catch { }
                });
            }

            return item;
        }
    }
}
