using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<IEnumerable<object>> GetTeachersAsync(int subjectId);
        Task<Subject> CreateAsync(Subject entity);
        Task<Subject?> UpdateAsync(int id, Subject entity);
        Task<bool> DeleteAsync(int id);
    }

    public class SubjectService : ISubjectService
    {
        private readonly EduPlusDbContext _context;

        public SubjectService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.Subjects.AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTeachersAsync(int subjectId)
        {
            return await _context.SubjectTeachers.AsNoTracking()
                .Where(st => st.SubjectId == subjectId)
                .Include(st => st.Teacher)
                .Select(st => new
                {
                    st.Teacher.Id,
                    st.Teacher.FirstName,
                    st.Teacher.LastName,
                    st.Teacher.Email
                })
                .ToListAsync();
        }

        public async Task<Subject> CreateAsync(Subject entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.Subjects.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Subject?> UpdateAsync(int id, Subject entity)
        {
            if (id != entity.Id) return null;

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

            _context.Subjects.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
