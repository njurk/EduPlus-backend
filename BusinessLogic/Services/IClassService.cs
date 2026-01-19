using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IClassService
    {
        Task<IEnumerable<object>> GetAllAsync(int? schoolYearId, bool includeInactive, string? sortBy = null, bool sortDesc = false, int? level = null);
        Task<object?> GetDetailsAsync(int id, string sortBy, bool sortDesc, string studentSearch, string subjectSearch, string subjectSortBy, bool subjectSortDesc);
        Task<IEnumerable<object>> GetCandidatesAsync(int classId, string search);
        Task AddStudentsBulkAsync(int classId, List<int> studentIds);
        Task RemoveStudentAsync(int classStudentId);
        Task RemoveSubjectAsync(int classSubjectId);
        Task<Class> CreateAsync(Class entity);
        Task AssignSubjectAsync(int classId, int subjectId, int teacherId);
        Task<Class> UpdateAsync(int id, Class entity);
        Task<bool> DeleteAsync(int id);
    }

    public class ClassService : IClassService
    {
        private readonly EduPlusDbContext _context;

        public ClassService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(int? schoolYearId, bool includeInactive, string? sortBy = null, bool sortDesc = false, int? level = null)
        {
            var query = _context.Classes.AsNoTracking();

            if (schoolYearId.HasValue)
                query = query.Where(c => c.SchoolYearId == schoolYearId);

            if (!includeInactive)
                query = query.Where(c => c.IsActive);

            if (level.HasValue)
                query = query.Where(c => c.Level == level.Value);

            var projected = query
                .Include(c => c.ClassStudents)
                .Select(c => new
                {
                    c.Id,
                    c.Level,
                    c.Letter,
                    c.SchoolYearId,
                    c.IsActive,
                    c.CreatedAt,
                    c.UpdatedAt,
                    StudentCount = c.ClassStudents.Count
                });

            projected = sortBy?.ToLower() switch
            {
                "class" => sortDesc 
                    ? projected.OrderByDescending(c => c.Level).ThenByDescending(c => c.Letter) 
                    : projected.OrderBy(c => c.Level).ThenBy(c => c.Letter),
                "studentcount" => sortDesc 
                    ? projected.OrderByDescending(c => c.StudentCount) 
                    : projected.OrderBy(c => c.StudentCount),
                "created" => sortDesc 
                    ? projected.OrderByDescending(c => c.CreatedAt) 
                    : projected.OrderBy(c => c.CreatedAt),
                "updated" => sortDesc 
                    ? projected.OrderByDescending(c => c.UpdatedAt) 
                    : projected.OrderBy(c => c.UpdatedAt),
                _ => projected.OrderBy(c => c.Level).ThenBy(c => c.Letter)
            };

            return await projected.ToListAsync();
        }

        public async Task<object?> GetDetailsAsync(int id, string sortBy, bool sortDesc, string studentSearch, string subjectSearch, string subjectSortBy, bool subjectSortDesc)
        {
            var classEntity = await _context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (classEntity == null) return null;

            var studentsQuery = _context.ClassStudents.AsNoTracking()
                .Where(cs => cs.ClassId == id)
                .Include(cs => cs.Student)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(studentSearch))
            {
                var s = studentSearch.Trim().ToLower();
                studentsQuery = studentsQuery.Where(cs =>
                    cs.Student.LastName.ToLower().Contains(s) ||
                    cs.Student.FirstName.ToLower().Contains(s) ||
                    cs.Student.Email.ToLower().Contains(s));
            }

            studentsQuery = sortBy?.ToLower() switch
            {
                "name" => sortDesc 
                    ? studentsQuery.OrderByDescending(x => x.Student.LastName).ThenByDescending(x => x.Student.FirstName)
                    : studentsQuery.OrderBy(x => x.Student.LastName).ThenBy(x => x.Student.FirstName),
                "email" => sortDesc ? studentsQuery.OrderByDescending(x => x.Student.Email) : studentsQuery.OrderBy(x => x.Student.Email),
                "createdat" => sortDesc ? studentsQuery.OrderByDescending(x => x.CreatedAt) : studentsQuery.OrderBy(x => x.CreatedAt),
                _ => sortDesc ? studentsQuery.OrderByDescending(x => x.OrderNumber) : studentsQuery.OrderBy(x => x.OrderNumber),
            };

            var students = await studentsQuery.Select(cs => new
            {
                cs.Id,
                cs.StudentId,
                cs.OrderNumber,
                cs.CreatedAt,
                cs.UpdatedAt,
                ModifiedByName = cs.ModifiedByUserId != null 
                    ? _context.Users.Where(u => u.Id == cs.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()
                    : "System",
                Student = new { cs.Student.Id, cs.Student.FirstName, cs.Student.LastName, cs.Student.Email }
            }).ToListAsync();

            var subjectsQuery = _context.ClassSubjects.AsNoTracking()
                .Where(cs => cs.ClassId == id)
                .Include(cs => cs.Subject)
                .AsQueryable();

            var rawSubjects = await subjectsQuery.Select(cs => new
            {
                cs.Id,
                cs.ClassId,
                cs.SubjectId,
                SubjectName = cs.Subject.Name,
                cs.CreatedAt,
                cs.UpdatedAt,
                ModifiedByName = cs.ModifiedByUserId != null 
                    ? _context.Users.Where(u => u.Id == cs.ModifiedByUserId).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault()
                    : "System",
                TeacherInfo = _context.TeacherClassSubjects
                    .Where(t => t.ClassId == id && t.SubjectId == cs.SubjectId)
                    .Select(t => new { t.TeacherId, Name = t.Teacher.LastName + " " + t.Teacher.FirstName })
                    .FirstOrDefault()
            }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(subjectSearch))
            {
                var s = subjectSearch.Trim().ToLower();
                rawSubjects = rawSubjects.Where(x => 
                    x.SubjectName.ToLower().Contains(s) || 
                    (x.TeacherInfo?.Name?.ToLower().Contains(s) ?? false)
                ).ToList();
            }

            var subjects = (subjectSortBy?.ToLower() switch
            {
                "teachername" => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.TeacherInfo?.Name) : rawSubjects.OrderBy(x => x.TeacherInfo?.Name),
                "createdat" => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.CreatedAt) : rawSubjects.OrderBy(x => x.CreatedAt),
                _ => subjectSortDesc ? rawSubjects.OrderByDescending(x => x.SubjectName) : rawSubjects.OrderBy(x => x.SubjectName)
            }).Select(s => new
            {
                s.Id,
                s.ClassId,
                s.SubjectId,
                s.SubjectName,
                s.CreatedAt,
                s.UpdatedAt,
                s.ModifiedByName,
                TeacherId = s.TeacherInfo?.TeacherId,
                TeacherName = s.TeacherInfo?.Name
            });

            return new
            {
                ClassInfo = classEntity,
                Students = students,
                Subjects = subjects
            };
        }

        public async Task<IEnumerable<object>> GetCandidatesAsync(int classId, string search)
        {
            var query = _context.Users.AsNoTracking()
                .Where(u => u.IsActive && u.UserRoles.Any(ur => ur.Role.Name == "Uczeń"))
                .Where(u => !u.ClassStudents.Any(cs => cs.ClassId == classId));

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(u =>
                    u.LastName.ToLower().Contains(s) ||
                    u.FirstName.ToLower().Contains(s) ||
                    u.Email.ToLower().Contains(s));
            }

            return await query
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .Take(50)
                .Select(u => new { u.Id, u.FirstName, u.LastName, u.Email })
                .ToListAsync();
        }

        public async Task<Class> CreateAsync(Class entity)
        {
            var exists = await _context.Classes.AnyAsync(c =>
                c.SchoolYearId == entity.SchoolYearId &&
                c.Level == entity.Level &&
                c.Letter == entity.Letter
            );

            if (exists) throw new InvalidOperationException($"Klasa {entity.Level}{entity.Letter} już istnieje w tym roku szkolnym");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.IsActive = true;

            _context.Classes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddStudentsBulkAsync(int classId, List<int> studentIds)
        {
            var newRelations = new List<ClassStudent>();
            foreach (var studentId in studentIds)
            {
                if (!await _context.ClassStudents.AnyAsync(cs => cs.ClassId == classId && cs.StudentId == studentId))
                {
                    newRelations.Add(new ClassStudent
                    {
                        ClassId = classId,
                        StudentId = studentId,
                        OrderNumber = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
            }

            if (newRelations.Any())
            {
                _context.ClassStudents.AddRange(newRelations);
                await _context.SaveChangesAsync();

                await RecalculateOrderNumbersAsync(classId);
            }
        }

        private async Task RecalculateOrderNumbersAsync(int classId)
        {
            var allStudents = await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Student)
                .OrderBy(cs => cs.Student.LastName)
                .ThenBy(cs => cs.Student.FirstName)
                .ToListAsync();

            int order = 1;
            foreach (var cs in allStudents)
            {
                cs.OrderNumber = order++;
                cs.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AssignSubjectAsync(int classId, int subjectId, int teacherId)
        {
            var isAuthorized = await _context.SubjectTeachers
                .AnyAsync(st => st.SubjectId == subjectId && st.TeacherId == teacherId);

            if (!isAuthorized)
            {
                throw new InvalidOperationException("Wybrany nauczyciel nie ma uprawnień do nauczania tego przedmiotu");
            }

            var exists = await _context.ClassSubjects
                .AnyAsync(cs => cs.ClassId == classId && cs.SubjectId == subjectId);

            if (exists)
            {
                throw new InvalidOperationException("Ten przedmiot jest już przypisany do tej klasy");
            }

            var classSubject = new ClassSubject
            {
                ClassId = classId,
                SubjectId = subjectId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.ClassSubjects.Add(classSubject);

            var teacherAssignment = new TeacherClassSubject
            {
                ClassId = classId,
                SubjectId = subjectId,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.TeacherClassSubjects.Add(teacherAssignment);

            await _context.SaveChangesAsync();
        }

        public async Task<Class> UpdateAsync(int id, Class entity)
        {
            if (id != entity.Id) throw new ArgumentException("Złe ID");

            var exists = await _context.Classes.AnyAsync(c =>
                c.SchoolYearId == entity.SchoolYearId &&
                c.Level == entity.Level &&
                c.Letter == entity.Letter &&
                c.Id != id
            );

            if (exists) throw new InvalidOperationException($"Klasa {entity.Level}{entity.Letter} już istnieje w tym roku.");

            var dbClass = await _context.Classes.FindAsync(id);
            if (dbClass == null) throw new KeyNotFoundException("Klasa nie znaleziona");

            dbClass.Level = entity.Level;
            dbClass.Letter = entity.Letter;
            dbClass.IsActive = entity.IsActive;
            dbClass.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return dbClass;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return false;

            if (classEntity.IsActive)
            {
                classEntity.IsActive = false;
                classEntity.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.Classes.Remove(classEntity);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveStudentAsync(int classStudentId)
        {
            var classStudent = await _context.ClassStudents.FindAsync(classStudentId);
            if (classStudent == null)
                throw new KeyNotFoundException("Nie znaleziono przypisania ucznia.");

            var classId = classStudent.ClassId;
            _context.ClassStudents.Remove(classStudent);
            await _context.SaveChangesAsync();

            await RecalculateOrderNumbersAsync(classId);
        }

        public async Task RemoveSubjectAsync(int classSubjectId)
        {
            var classSubject = await _context.ClassSubjects.FindAsync(classSubjectId);
            if (classSubject == null)
                throw new KeyNotFoundException("Nie znaleziono przypisania przedmiotu.");

            var teacherAssignments = await _context.TeacherClassSubjects
                .Where(t => t.ClassId == classSubject.ClassId && t.SubjectId == classSubject.SubjectId)
                .ToListAsync();

            _context.TeacherClassSubjects.RemoveRange(teacherAssignments);
            _context.ClassSubjects.Remove(classSubject);
            await _context.SaveChangesAsync();
        }
    }
}
