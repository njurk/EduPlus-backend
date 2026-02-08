using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IGradeColumnService
    {
        Task<IEnumerable<object>> GetAllAsync(int classId, int subjectId, int semesterId);
        Task<object> CreateAsync(int classId, int subjectId, int semesterId, int gradeCategoryId, int teacherId, string? name);
        Task<object?> UpdateAsync(int id, string? name, int gradeCategoryId);
        Task<bool> DeleteAsync(int id);
    }

    public class GradeColumnService : IGradeColumnService
    {
        private readonly EduPlusDbContext _context;

        public GradeColumnService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(int classId, int subjectId, int semesterId)
        {
            return await _context.GradeColumns.AsNoTracking()
                .Where(gc => gc.ClassId == classId && gc.SubjectId == subjectId && gc.SemesterId == semesterId)
                .OrderBy(gc => gc.Order)
                .Select(gc => new
                {
                    gc.Id,
                    gc.Name,
                    gc.ClassId,
                    gc.SubjectId,
                    gc.SemesterId,
                    gc.GradeCategoryId,
                    CategoryName = gc.GradeCategory!.Name,
                    CategoryColor = gc.GradeCategory.ColorHex,
                    gc.TeacherId,
                    gc.Order
                })
                .ToListAsync();
        }

        public async Task<object> CreateAsync(int classId, int subjectId, int semesterId, int gradeCategoryId, int teacherId, string? name)
        {
            var maxOrder = await _context.GradeColumns
                .Where(gc => gc.ClassId == classId && gc.SubjectId == subjectId && gc.SemesterId == semesterId)
                .MaxAsync(gc => (int?)gc.Order) ?? 0;

            var entity = new GradeColumn
            {
                Name = name,
                ClassId = classId,
                SubjectId = subjectId,
                SemesterId = semesterId,
                GradeCategoryId = gradeCategoryId,
                TeacherId = teacherId,
                Order = maxOrder + 1
            };

            _context.GradeColumns.Add(entity);
            await _context.SaveChangesAsync();

            return new
            {
                entity.Id,
                entity.Name,
                entity.ClassId,
                entity.SubjectId,
                entity.SemesterId,
                entity.GradeCategoryId,
                CategoryName = (await _context.GradeCategories.FindAsync(gradeCategoryId))?.Name,
                CategoryColor = (await _context.GradeCategories.FindAsync(gradeCategoryId))?.ColorHex,
                entity.TeacherId,
                entity.Order
            };
        }

        public async Task<object?> UpdateAsync(int id, string? name, int gradeCategoryId)
        {
            var entity = await _context.GradeColumns.FindAsync(id);
            if (entity == null) return null;

            entity.Name = name;
            entity.GradeCategoryId = gradeCategoryId;
            await _context.SaveChangesAsync();

            var category = await _context.GradeCategories.FindAsync(gradeCategoryId);
            return new
            {
                entity.Id,
                entity.Name,
                entity.ClassId,
                entity.SubjectId,
                entity.SemesterId,
                entity.GradeCategoryId,
                CategoryName = category?.Name,
                CategoryColor = category?.ColorHex,
                entity.TeacherId,
                entity.Order
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.GradeColumns.FindAsync(id);
            if (entity == null) return false;

            if (await _context.Grades.AnyAsync(g => g.GradeColumnId == id))
                throw new InvalidOperationException("Nie można usunąć kolumny zawierającej oceny");

            _context.GradeColumns.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
