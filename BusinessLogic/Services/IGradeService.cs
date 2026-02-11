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
        Task<PaginatedResponse<object>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? semesterId = null, int? schoolYearId = null, int? subjectId = null, int? gradeTypeId = null, int? gradeCategoryId = null, int? teacherId = null, bool showInactive = false);
        Task<object?> GetByIdAsync(int id);
        Task<object> CreateAsync(GradeDto dto, int teacherId, IEmailService emailService);
        Task<object?> UpdateAsync(int id, GradeDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<IEnumerable<object>> GetTeacherAssignmentsAsync(int teacherId, int yearId);
        Task<object> CreateBulkAsync(BulkGradeCreateDto dto, int teacherId, IEmailService emailService);
        Task<object> UpsertSemesterGradesAsync(BulkGradeCreateDto dto, int teacherId);
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
                        .Where(g => g.StudentId == cs.StudentId && g.SubjectId == subjectId && g.IsActive && g.DateTime >= start && g.DateTime <= end)
                        .OrderBy(g => g.DateTime)
                        .Select(g => new
                        {
                            g.Id,
                            g.GradeTypeId,
                            g.GradeCategoryId,
                            g.GradeColumnId,
                            g.Comment,
                            g.CreatedAt,
                            GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                            GradeCategory = new { g.GradeCategory.Name, g.GradeCategory.ColorHex, g.GradeCategory.Slug },
                            TeacherName = g.Teacher.LastName + " " + g.Teacher.FirstName + (g.Teacher.IsActive ? "" : " (nieaktywny)")
                        }).ToList()
                })
                .OrderBy(x => x.OrderNumber).ToListAsync();
        }

        public async Task<PaginatedResponse<object>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, int? classId = null, int? semesterId = null, int? schoolYearId = null, int? subjectId = null, int? gradeTypeId = null, int? gradeCategoryId = null, int? teacherId = null, bool showInactive = false)
        {
            var query = _context.GradesAdminList.AsNoTracking().AsQueryable();

            query = query.Where(g => showInactive ? !g.IsActive : g.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(g =>
                    g.StudentName.ToLower().Contains(s) ||
                    g.SubjectName.ToLower().Contains(s) ||
                    g.GradeTypeName.ToLower().Contains(s) ||
                    g.TeacherName.ToLower().Contains(s));
            }

            if (classId.HasValue)
                query = query.Where(g => g.ClassId == classId.Value);

            if (semesterId.HasValue)
            {
                var semester = await _context.Semesters.FindAsync(semesterId.Value);
                if (semester != null)
                {
                    var startDate = semester.StartDate.ToDateTime(TimeOnly.MinValue);
                    var endDate = semester.EndDate.ToDateTime(TimeOnly.MaxValue);
                    query = query.Where(g => g.CreatedAt >= startDate && g.CreatedAt <= endDate);
                }
            }
            else if (schoolYearId.HasValue)
            {
                var schoolYear = await _context.SchoolYears.FindAsync(schoolYearId.Value);
                if (schoolYear != null)
                {
                    var startDate = schoolYear.StartDate.ToDateTime(TimeOnly.MinValue);
                    var endDate = schoolYear.EndDate.ToDateTime(TimeOnly.MaxValue);
                    query = query.Where(g => g.CreatedAt >= startDate && g.CreatedAt <= endDate);
                }
            }

            if (subjectId.HasValue)
                query = query.Where(g => g.SubjectId == subjectId.Value);

            if (gradeTypeId.HasValue)
                query = query.Where(g => g.GradeTypeId == gradeTypeId.Value);

            if (gradeCategoryId.HasValue)
                query = query.Where(g => g.GradeCategoryId == gradeCategoryId.Value);

            if (teacherId.HasValue)
                query = query.Where(g => g.TeacherId == teacherId.Value);

            var projected = query.Select(g => new
            {
                g.Id,
                StudentName = g.StudentName + (_context.Users.Where(u => u.Id == g.StudentId).Select(u => u.IsActive).FirstOrDefault() ? "" : " (nieaktywny)"),
                g.ClassName,
                SubjectName = g.SubjectName + (_context.Subjects.Where(s => s.Id == g.SubjectId).Select(s => s.IsActive).FirstOrDefault() ? "" : " (nieaktywny)"),
                GradeTypeName = g.GradeTypeName,
                GradeValue = g.GradeValue,
                CategoryName = g.CategoryName,
                CategoryColorHex = g.CategoryColorHex,
                TeacherName = g.TeacherName + (_context.Users.Where(u => u.Id == g.TeacherId).Select(u => u.IsActive).FirstOrDefault() ? "" : " (nieaktywny)"),
                g.Comment,
                g.CreatedAt,
                g.UpdatedAt,
                g.IsActive,
                g.ModifiedByName
            });

            projected = sortBy?.ToLower() switch
            {
                "studentname" => sortDesc ? projected.OrderByDescending(g => g.StudentName) : projected.OrderBy(g => g.StudentName),
                "classname" => sortDesc ? projected.OrderByDescending(g => g.ClassName) : projected.OrderBy(g => g.ClassName),
                "subjectname" => sortDesc ? projected.OrderByDescending(g => g.SubjectName) : projected.OrderBy(g => g.SubjectName),
                "gradevalue" => sortDesc ? projected.OrderByDescending(g => g.GradeValue) : projected.OrderBy(g => g.GradeValue),
                "categoryname" => sortDesc ? projected.OrderByDescending(g => g.CategoryName) : projected.OrderBy(g => g.CategoryName),
                "teachername" => sortDesc ? projected.OrderByDescending(g => g.TeacherName) : projected.OrderBy(g => g.TeacherName),
                "updatedat" => sortDesc ? projected.OrderByDescending(g => g.UpdatedAt) : projected.OrderBy(g => g.UpdatedAt),
                _ => sortDesc ? projected.OrderByDescending(g => g.CreatedAt) : projected.OrderBy(g => g.CreatedAt)
            };

            var totalCount = await projected.CountAsync();

            var data = await projected
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<object>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = data.Cast<object>().ToList()
            };
        }

        public async Task<object> CreateAsync(GradeDto dto, int teacherId, IEmailService emailService)
        {
            if (!await _context.GradeTypes.AnyAsync(gt => gt.Id == dto.GradeTypeId && gt.IsActive))
                throw new InvalidOperationException("Wybrany typ oceny nie istnieje lub jest nieaktywny");

            if (!await _context.GradeCategories.AnyAsync(gc => gc.Id == dto.GradeCategoryId && gc.IsActive))
                throw new InvalidOperationException("Wybrana kategoria oceny nie istnieje lub jest nieaktywna");

            var entity = new Grade
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                GradeTypeId = dto.GradeTypeId,
                GradeCategoryId = dto.GradeCategoryId,
                GradeColumnId = dto.GradeColumnId,
                Comment = dto.Comment,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Grades.Add(entity);
            await _context.SaveChangesAsync();

            var gradeData = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .Where(g => g.Id == entity.Id)
                .Select(g => new
                {
                    g.Id,
                    g.GradeTypeId,
                    g.GradeCategoryId,
                    g.Comment,
                    g.CreatedAt,
                    GradeType = new { g.GradeType.Numeric, g.GradeType.Name, g.GradeType.Value },
                    GradeCategory = new { g.GradeCategory.Name },
                    TeacherName = g.Teacher.LastName + " " + g.Teacher.FirstName + (g.Teacher.IsActive ? "" : " (nieaktywny)"),
                    StudentName = g.Student.LastName + " " + g.Student.FirstName + (g.Student.IsActive ? "" : " (nieaktywny)"),
                    StudentEmail = g.Student.Email,
                    SubjectName = g.Subject.Name + (g.Subject.IsActive ? "" : " (nieaktywny)"),
                    StudentId = g.StudentId
                })
                .FirstAsync();

            var gradeValue = $"{gradeData.GradeType.Numeric} ({gradeData.GradeType.Name})";
            _ = Task.Run(async () =>
            {
                try
                {
                    await emailService.SendNewGradeEmailAsync(gradeData.StudentEmail, gradeData.StudentName, gradeData.SubjectName, gradeValue, gradeData.TeacherName, gradeData.CreatedAt);
                    var parents = await _context.ParentStudents.Include(ps => ps.Parent).Where(ps => ps.StudentId == gradeData.StudentId).Select(ps => ps.Parent!.Email).ToListAsync();
                    foreach (var email in parents)
                        await emailService.SendNewGradeEmailAsync(email, gradeData.StudentName, gradeData.SubjectName, gradeValue, gradeData.TeacherName, gradeData.CreatedAt);
                }
                catch { }
            });

            return new { gradeData.Id, gradeData.GradeTypeId, gradeData.GradeCategoryId, gradeData.Comment, gradeData.CreatedAt, gradeData.GradeType, gradeData.GradeCategory, gradeData.TeacherName };
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
            existing.GradeColumnId = dto.GradeColumnId;
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
                TeacherName = existing.Teacher.LastName + " " + existing.Teacher.FirstName + (existing.Teacher.IsActive ? "" : " (nieaktywny)")
            };
        }

        public async Task<object?> GetByIdAsync(int id)
        {
            var grade = await _context.Grades.AsNoTracking()
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .Include(g => g.GradeType)
                .Include(g => g.GradeCategory)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null) return null;

            var className = await _context.ClassStudents
                .Where(cs => cs.StudentId == grade.StudentId)
                .Select(cs => cs.Class.Level + cs.Class.Letter)
                .FirstOrDefaultAsync();

            string? modifiedByName = null;
            if (grade.ModifiedByUserId.HasValue)
            {
                modifiedByName = await _context.Users
                    .Where(u => u.Id == grade.ModifiedByUserId.Value)
                    .Select(u => u.LastName + " " + u.FirstName)
                    .FirstOrDefaultAsync();
            }

            return new
            {
                grade.Id,
                StudentId = grade.StudentId,
                StudentName = grade.Student!.LastName + " " + grade.Student.FirstName + (grade.Student.IsActive ? "" : " (nieaktywny)"),
                ClassName = className,
                SubjectId = grade.SubjectId,
                SubjectName = grade.Subject!.Name + (grade.Subject.IsActive ? "" : " (nieaktywny)"),
                GradeTypeId = grade.GradeTypeId,
                GradeTypeName = grade.GradeType!.Numeric + " (" + grade.GradeType.Name + ")",
                GradeValue = grade.GradeType.Value,
                GradeCategoryId = grade.GradeCategoryId,
                CategoryName = grade.GradeCategory!.Name,
                TeacherId = grade.TeacherId,
                TeacherName = grade.Teacher!.LastName + " " + grade.Teacher.FirstName + (grade.Teacher!.IsActive ? "" : " (nieaktywny)"),
                grade.Comment,
                grade.CreatedAt,
                grade.UpdatedAt,
                grade.IsActive,
                ModifiedByName = modifiedByName
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Grades.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.Grades.Remove(item);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Grades.IgnoreQueryFilters().FirstOrDefaultAsync(g => g.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<object>> GetTeacherAssignmentsAsync(int teacherId, int yearId)
        {
            return await _context.TeacherAssignmentsList.AsNoTracking()
                .Where(ta => ta.TeacherId == teacherId
                    && _context.Classes.Any(c => c.Id == ta.ClassId && c.SchoolYearId == yearId))
                .Select(ta => new
                {
                    ta.ClassId,
                    ta.ClassName,
                    ta.SubjectId,
                    ta.SubjectName
                })
                .OrderBy(x => x.ClassName)
                .ThenBy(x => x.SubjectName)
                .ToListAsync();
        }

        public async Task<object> CreateBulkAsync(BulkGradeCreateDto dto, int teacherId, IEmailService emailService)
        {
            if (!await _context.GradeCategories.AnyAsync(gc => gc.Id == dto.GradeCategoryId && gc.IsActive))
                throw new InvalidOperationException("Wybrana kategoria oceny nie istnieje lub jest nieaktywna");

            var gradesJson = System.Text.Json.JsonSerializer.Serialize(
                dto.Grades.Select(g => new { studentId = g.StudentId, gradeTypeId = g.GradeTypeId }));

            var columnParam = new Microsoft.Data.SqlClient.SqlParameter("@GradeColumnId", System.Data.SqlDbType.Int)
            { Value = (object?)dto.GradeColumnId ?? DBNull.Value };

            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_BulkInsertGrades @SubjectId = {0}, @GradeCategoryId = {1}, @GradeColumnId = @GradeColumnId, @TeacherId = {2}, @GradesJson = {3}",
                dto.SubjectId, dto.GradeCategoryId, teacherId, gradesJson, columnParam);

            _ = Task.Run(async () =>
            {
                try
                {
                    var cutoff = DateTime.Now.AddMinutes(-1);
                    var gradeData = await _context.Grades.AsNoTracking()
                        .Include(g => g.Student).Include(g => g.Subject)
                        .Include(g => g.GradeType).Include(g => g.Teacher)
                        .Where(g => g.TeacherId == teacherId && g.SubjectId == dto.SubjectId && g.CreatedAt >= cutoff)
                        .Select(g => new
                        {
                            g.StudentId,
                            StudentEmail = g.Student.Email,
                            StudentName = g.Student.LastName + " " + g.Student.FirstName,
                            SubjectName = g.Subject.Name,
                            GradeValue = g.GradeType.Numeric + " (" + g.GradeType.Name + ")",
                            TeacherName = g.Teacher.LastName + " " + g.Teacher.FirstName,
                            g.CreatedAt
                        }).ToListAsync();

                    foreach (var gd in gradeData)
                    {
                        await emailService.SendNewGradeEmailAsync(gd.StudentEmail, gd.StudentName, gd.SubjectName, gd.GradeValue, gd.TeacherName, gd.CreatedAt);
                        var parents = await _context.ParentStudents.Include(ps => ps.Parent)
                            .Where(ps => ps.StudentId == gd.StudentId).Select(ps => ps.Parent!.Email).ToListAsync();
                        foreach (var email in parents)
                            await emailService.SendNewGradeEmailAsync(email, gd.StudentName, gd.SubjectName, gd.GradeValue, gd.TeacherName, gd.CreatedAt);
                    }
                }
                catch { }
            });

            return new { count = dto.Grades.Count };
        }

        public async Task<object> UpsertSemesterGradesAsync(BulkGradeCreateDto dto, int teacherId)
        {
            var studentIds = dto.Grades.Select(g => g.StudentId).ToList();
            var existing = await _context.Grades
                .Where(g => g.SubjectId == dto.SubjectId && g.GradeCategoryId == dto.GradeCategoryId
                    && studentIds.Contains(g.StudentId) && g.IsActive)
                .ToListAsync();

            var existingMap = existing.ToDictionary(g => g.StudentId);
            int updated = 0, created = 0, deleted = 0;

            foreach (var item in dto.Grades)
            {
                if (existingMap.TryGetValue(item.StudentId, out var grade))
                {
                    if (item.GradeTypeId == 0)
                    {
                        grade.IsActive = false;
                        grade.UpdatedAt = DateTime.Now;
                        deleted++;
                    }
                    else
                    {
                        grade.GradeTypeId = item.GradeTypeId;
                        grade.UpdatedAt = DateTime.Now;
                        updated++;
                    }
                }
                else if (item.GradeTypeId > 0)
                {
                    _context.Grades.Add(new Data.Data.Entities.Grade
                    {
                        StudentId = item.StudentId,
                        SubjectId = dto.SubjectId,
                        GradeTypeId = item.GradeTypeId,
                        GradeCategoryId = dto.GradeCategoryId,
                        TeacherId = teacherId,
                        DateTime = DateTime.Now,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                    created++;
                }
            }

            await _context.SaveChangesAsync();
            return new { updated, created, deleted };
        }
    }
}
