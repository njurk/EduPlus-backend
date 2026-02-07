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
    public interface ISubjectService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false);
        Task<IEnumerable<object>> GetTeachersAsync(int subjectId);
        Task<PaginatedResponse<object>> GetAllSubjectTeachersAsync(int pageNumber, int pageSize, string? sortBy, bool sortDesc, string? search, int? subjectId, int? teacherId, bool showInactive);
        Task AddTeacherToSubjectAsync(int subjectId, int teacherId, int userId);
        Task RemoveTeacherFromSubjectAsync(int subjectId, int teacherId, int userId);
        Task RestoreTeacherSubjectAsync(int subjectId, int teacherId, int userId);
        Task UpdateSubjectTeacherAsync(int subjectId, int oldTeacherId, int newTeacherId, int userId);
        Task<Subject> CreateAsync(Subject entity);
        Task<Subject?> UpdateAsync(int id, Subject entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }

    public class SubjectService : ISubjectService
    {
        private readonly EduPlusDbContext _context;

        public SubjectService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search = null, string? sortBy = null, bool sortDesc = false, bool showInactive = false)
        {
            var query = _context.Subjects.AsNoTracking().AsQueryable();

            query = showInactive ? query.Where(x => !x.IsActive) : query.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
                "created" => sortDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
                "updated" => sortDesc ? query.OrderByDescending(x => x.UpdatedAt) : query.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name)
            };

            return await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.IsActive,
                    x.CreatedAt,
                    x.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == x.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTeachersAsync(int subjectId)
        {
            return await _context.SubjectTeachers.AsNoTracking()
                .Where(st => st.SubjectId == subjectId && st.IsActive)
                .Include(st => st.Teacher)
                .OrderBy(st => st.Teacher.LastName)
                .ThenBy(st => st.Teacher.FirstName)
                .Select(st => new
                {
                    st.Teacher.Id,
                    st.Teacher.FirstName,
                    st.Teacher.LastName,
                    st.Teacher.Email
                })
                .ToListAsync();
        }

        public async Task<PaginatedResponse<object>> GetAllSubjectTeachersAsync(int pageNumber, int pageSize, string? sortBy, bool sortDesc, string? search, int? subjectId, int? teacherId, bool showInactive)
        {
            var query = _context.SubjectTeachers.AsNoTracking()
                .Include(st => st.Subject)
                .Include(st => st.Teacher)
                .Where(st => showInactive || st.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower().Trim();
                query = query.Where(st =>
                    st.Subject.Name.ToLower().Contains(s) ||
                    st.Teacher.LastName.ToLower().Contains(s) ||
                    st.Teacher.FirstName.ToLower().Contains(s));
            }

            if (subjectId.HasValue) query = query.Where(st => st.SubjectId == subjectId.Value);
            if (teacherId.HasValue) query = query.Where(st => st.TeacherId == teacherId.Value);

            var projected = query.Select(st => new
            {
                st.SubjectId,
                st.TeacherId,
                SubjectName = st.Subject.Name + (st.Subject.IsActive ? "" : " (nieaktywny)"),
                TeacherName = st.Teacher.LastName + " " + st.Teacher.FirstName + (st.Teacher.IsActive ? "" : " (nieaktywny)"),
                st.CreatedAt,
                st.UpdatedAt,
                st.IsActive,
                ModifiedByName = st.ModifiedByUserId != null
                    ? _context.Users.Where(u => u.Id == st.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault()
                    : "System"
            });

            projected = sortBy?.ToLower() switch
            {
                "teachername" => sortDesc ? projected.OrderByDescending(x => x.TeacherName) : projected.OrderBy(x => x.TeacherName),
                "createdat" => sortDesc ? projected.OrderByDescending(x => x.CreatedAt) : projected.OrderBy(x => x.CreatedAt),
                "updatedat" => sortDesc ? projected.OrderByDescending(x => x.UpdatedAt) : projected.OrderBy(x => x.UpdatedAt),
                _ => sortDesc ? projected.OrderByDescending(x => x.SubjectName) : projected.OrderBy(x => x.SubjectName)
            };

            var totalCount = await projected.CountAsync();
            var data = await projected.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResponse<object>
            {
                Data = data.Cast<object>().ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task AddTeacherToSubjectAsync(int subjectId, int teacherId, int userId)
        {
            var existing = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == subjectId && st.TeacherId == teacherId);
            if (existing != null)
            {
                if (existing.IsActive)
                    throw new InvalidOperationException("Ten nauczyciel jest już przypisany do tego przedmiotu");
                
                existing.IsActive = true;
                existing.UpdatedAt = DateTime.Now;
                existing.ModifiedByUserId = userId;
                await _context.SaveChangesAsync();
                return;
            }

            var teacher = await _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == teacherId && u.IsActive);
            if (teacher == null || !teacher.UserRoles.Any(ur => ur.Role.Level == 2))
                throw new InvalidOperationException("Wybrany użytkownik nie jest nauczycielem");

            _context.SubjectTeachers.Add(new SubjectTeacher
            {
                SubjectId = subjectId,
                TeacherId = teacherId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ModifiedByUserId = userId
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTeacherFromSubjectAsync(int subjectId, int teacherId, int userId)
        {
            var relation = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == subjectId && st.TeacherId == teacherId);
            if (relation == null) throw new KeyNotFoundException("Nie znaleziono przypisania");

            relation.IsActive = false;
            relation.UpdatedAt = DateTime.Now;
            relation.ModifiedByUserId = userId;

            var relatedClassAssignments = await _context.TeacherClassSubjects
                .Where(tcs => tcs.SubjectId == subjectId && tcs.TeacherId == teacherId && tcs.IsActive)
                .ToListAsync();

            foreach (var assignment in relatedClassAssignments)
            {
                assignment.IsActive = false;
                assignment.UpdatedAt = DateTime.Now;
                assignment.ModifiedByUserId = userId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RestoreTeacherSubjectAsync(int subjectId, int teacherId, int userId)
        {
            var relation = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == subjectId && st.TeacherId == teacherId);
            if (relation == null) throw new KeyNotFoundException("Nie znaleziono przypisania");

            relation.IsActive = true;
            relation.UpdatedAt = DateTime.Now;
            relation.ModifiedByUserId = userId;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubjectTeacherAsync(int subjectId, int oldTeacherId, int newTeacherId, int userId)
        {
            var relation = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == subjectId && st.TeacherId == oldTeacherId);
            if (relation == null) throw new KeyNotFoundException("Nie znaleziono przypisania");

            var existingNewRelation = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == subjectId && st.TeacherId == newTeacherId);
            if (existingNewRelation != null && existingNewRelation.IsActive)
                throw new InvalidOperationException("Ten nauczyciel jest już przypisany do tego przedmiotu");

            relation.TeacherId = newTeacherId;
            relation.UpdatedAt = DateTime.Now;
            relation.ModifiedByUserId = userId;

            var classAssignments = await _context.TeacherClassSubjects
                .Where(tcs => tcs.SubjectId == subjectId && tcs.TeacherId == oldTeacherId)
                .ToListAsync();

            foreach (var assignment in classAssignments)
            {
                assignment.TeacherId = newTeacherId;
                assignment.UpdatedAt = DateTime.Now;
                assignment.ModifiedByUserId = userId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Subject> CreateAsync(Subject entity)
        {
            if (await _context.Subjects.AnyAsync(x => x.Name == entity.Name && x.IsActive))
                throw new InvalidOperationException("Ta nazwa przedmiotu już istnieje");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.Subjects.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Subject?> UpdateAsync(int id, Subject entity)
        {
            if (id != entity.Id) return null;

            if (await _context.Subjects.AnyAsync(x => x.Name == entity.Name && x.Id != id && x.IsActive))
                throw new InvalidOperationException("Ta nazwa przedmiotu już istnieje");

            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            entity.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Subjects.AnyAsync(e => e.Id == id)) return null;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.Subjects.FindAsync(id);
            if (item == null) return false;

            if (item.IsActive)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTime.Now;
            }
            else
            {
                _context.Subjects.Remove(item);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var item = await _context.Subjects.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            item.IsActive = true;
            item.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

