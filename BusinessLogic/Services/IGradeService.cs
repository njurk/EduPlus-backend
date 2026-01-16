using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IGradeService
    {
        Task<int> GetCurrentSemesterAsync(int schoolYearId);
        Task<IEnumerable<object>> GetClassGradesAsync(int classId, int subjectId, int semester, int? schoolYearId);
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, int? classId, bool showInactive);
        Task<object> CreateAsync(GradeDto dto, int teacherId);
        Task<object?> UpdateAsync(int id, GradeDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class GradeService : IGradeService
    {
        private readonly EduPlusDbContext _context;

        public GradeService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCurrentSemesterAsync(int schoolYearId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var semesters = await _context.Semesters.AsNoTracking()
                .Where(s => s.SchoolYearId == schoolYearId).OrderBy(s => s.StartDate).ToListAsync();
            var current = semesters.FirstOrDefault(s => s.StartDate <= today && s.EndDate >= today);
            return current != null ? semesters.IndexOf(current) + 1 : 1;
        }

        public async Task<IEnumerable<object>> GetClassGradesAsync(int classId, int subjectId, int semester, int? schoolYearId)
        {
            var yearId = schoolYearId ?? await _context.SchoolYears.Where(y => y.IsActive).Select(y => y.Id).FirstOrDefaultAsync();
            if (yearId == 0) throw new InvalidOperationException("Brak aktywnego roku");

            var semesters = await _context.Semesters.AsNoTracking()
                .Where(s => s.SchoolYearId == yearId).OrderBy(s => s.StartDate).ToListAsync();
            var targetSem = semesters.ElementAtOrDefault(semester - 1);
            if (targetSem == null) throw new ArgumentException("Nieprawidlowy numer semestru");

            var start = targetSem.StartDate.ToDateTime(TimeOnly.MinValue);
            var end = targetSem.EndDate.ToDateTime(TimeOnly.MaxValue);

            return await _context.ClassStudents.AsNoTracking()
                .Where(cs => cs.ClassId == classId)
                .Select(cs => new
                {
                    cs.Id,
                    cs.StudentId,
                    cs.Student.FirstName,
                    cs.Student.LastName,
                    cs.OrderNumber,
                    Average = EduPlusDbContext.CalculateWeightedAverage(cs.StudentId, subjectId, start, end),
                    Grades = _context.Grades
                        .Where(g => g.StudentId == cs.StudentId && g.SubjectId == subjectId && g.IsActive && g.CreatedAt >= start && g.CreatedAt <= end)
                        .OrderBy(g => g.CreatedAt)
                        .Select(g => new
                        {
                            g.Id,
                            g.GradeTypeId,
                            g.GradeCategoryId,
                            g.Comment,
                            g.CreatedAt,
                            GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                            GradeCategory = new { g.GradeCategory.Name },
                            TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                        }).ToList()
                })
                .OrderBy(x => x.OrderNumber).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc, int? classId, bool showInactive)
        {
            var query = _context.Grades.AsNoTracking()
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .AsQueryable();

            if (!showInactive) query = query.Where(g => g.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(g =>
                    g.Student.LastName.ToLower().Contains(s) ||
                    g.Student.FirstName.ToLower().Contains(s) ||
                    g.Subject.Name.ToLower().Contains(s) ||
                    g.GradeType.Name.ToLower().Contains(s));
            }

            if (classId.HasValue)
            {
                var studentIds = await _context.ClassStudents
                    .Where(cs => cs.ClassId == classId.Value)
                    .Select(cs => cs.StudentId)
                    .ToListAsync();
                query = query.Where(g => studentIds.Contains(g.StudentId));
            }

            var projected = query.Select(g => new
            {
                g.Id,
                StudentName = g.Student.LastName + " " + g.Student.FirstName,
                ClassName = _context.ClassStudents
                    .Where(cs => cs.StudentId == g.StudentId)
                    .Select(cs => cs.Class.Level + cs.Class.Letter)
                    .FirstOrDefault(),
                SubjectName = g.Subject.Name,
                GradeTypeName = g.GradeType.Name,
                GradeValue = g.GradeType.Value,
                CategoryName = g.GradeCategory.Name,
                g.Comment,
                g.CreatedAt,
                g.UpdatedAt,
                g.IsActive,
                TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
            });

            projected = sortBy?.ToLower() switch
            {
                "studentname" => sortDesc ? projected.OrderByDescending(g => g.StudentName) : projected.OrderBy(g => g.StudentName),
                "subjectname" => sortDesc ? projected.OrderByDescending(g => g.SubjectName) : projected.OrderBy(g => g.SubjectName),
                "gradevalue" => sortDesc ? projected.OrderByDescending(g => g.GradeValue) : projected.OrderBy(g => g.GradeValue),
                "updatedat" => sortDesc ? projected.OrderByDescending(g => g.UpdatedAt) : projected.OrderBy(g => g.UpdatedAt),
                _ => sortDesc ? projected.OrderByDescending(g => g.CreatedAt) : projected.OrderBy(g => g.CreatedAt)
            };

            return await projected.ToListAsync();
        }

        public async Task<object> CreateAsync(GradeDto dto, int teacherId)
        {
            var entity = new Grade
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                GradeTypeId = dto.GradeTypeId,
                GradeCategoryId = dto.GradeCategoryId,
                Comment = dto.Comment,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Grades.Add(entity);
            await _context.SaveChangesAsync();

            return await _context.Grades
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .Select(g => new
                {
                    g.Id,
                    g.GradeTypeId,
                    g.GradeCategoryId,
                    g.Comment,
                    g.CreatedAt,
                    GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                    GradeCategory = new { g.GradeCategory.Name },
                    TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                })
                .FirstOrDefaultAsync(g => g.Id == entity.Id);
        }

        public async Task<object?> UpdateAsync(int id, GradeDto dto)
        {
            var existing = await _context.Grades
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (existing == null) return null;

            existing.GradeTypeId = dto.GradeTypeId;
            existing.GradeCategoryId = dto.GradeCategoryId;
            existing.Comment = dto.Comment;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            if (dto.GradeTypeId != existing.GradeTypeId) await _context.Entry(existing).Reference(g => g.GradeType).LoadAsync();
            if (dto.GradeCategoryId != existing.GradeCategoryId) await _context.Entry(existing).Reference(g => g.GradeCategory).LoadAsync();

            return new
            {
                existing.Id,
                existing.GradeTypeId,
                existing.GradeCategoryId,
                existing.Comment,
                existing.CreatedAt,
                GradeType = new { existing.GradeType.Numeric, existing.GradeType.Name, existing.GradeType.Value },
                GradeCategory = new { existing.GradeCategory.Name },
                TeacherName = existing.Teacher.FirstName + " " + existing.Teacher.LastName
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Grades.FindAsync(id);
            if (item == null) return false;
            item.IsActive = false;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
