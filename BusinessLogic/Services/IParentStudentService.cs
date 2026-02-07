using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IParentStudentService
    {
        Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc);
        Task<ParentStudent> CreateAsync(ParentStudent entity);
        Task<bool> DeleteAsync(int id);
    }

    public class ParentStudentService : IParentStudentService
    {
        private readonly EduPlusDbContext _context;

        public ParentStudentService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(string? search, string? sortBy, bool sortDesc)
        {
            var query = _context.ParentStudents.AsNoTracking()
                .Include(ps => ps.Parent)
                .Include(ps => ps.Student)
                .AsQueryable();

            var users = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(ps =>
                    ps.Parent.LastName.ToLower().Contains(s) ||
                    ps.Parent.FirstName.ToLower().Contains(s) ||
                    ps.Student.LastName.ToLower().Contains(s) ||
                    ps.Student.FirstName.ToLower().Contains(s));
            }

            var projected = query.Select(ps => new
            {
                ps.Id,
                ps.ParentId,
                ps.StudentId,
                ParentName = ps.Parent.LastName + " " + ps.Parent.FirstName + (ps.Parent.IsActive ? "" : " (nieaktywny)"),
                StudentName = ps.Student.LastName + " " + ps.Student.FirstName + (ps.Student.IsActive ? "" : " (nieaktywny)"),
                ps.CreatedAt,
                ps.UpdatedAt,
                ModifiedByName = ps.ModifiedByUserId != null 
                    ? users.Where(u => u.Id == ps.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() 
                    : null
            });

            projected = sortBy?.ToLower() switch
            {
                "parentname" => sortDesc ? projected.OrderByDescending(x => x.ParentName) : projected.OrderBy(x => x.ParentName),
                "studentname" => sortDesc ? projected.OrderByDescending(x => x.StudentName) : projected.OrderBy(x => x.StudentName),
                _ => sortDesc ? projected.OrderByDescending(x => x.CreatedAt) : projected.OrderBy(x => x.CreatedAt)
            };

            return await projected.ToListAsync();
        }

        public async Task<ParentStudent> CreateAsync(ParentStudent entity)
        {
            if (await _context.ParentStudents.AnyAsync(ps => ps.ParentId == entity.ParentId && ps.StudentId == entity.StudentId))
                throw new InvalidOperationException("Ta relacja już istnieje");

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            _context.ParentStudents.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.ParentStudents.FindAsync(id);
            if (item == null) return false;

            _context.ParentStudents.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
