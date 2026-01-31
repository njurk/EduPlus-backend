using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IExcuseService
    {
        Task<PaginatedResponse<ExcuseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false);
        Task<ExcuseDto?> GetByIdAsync(int id);
        Task<Excuse> CreateAsync(CreateExcuseDto dto, int parentId);
        Task<bool> AcceptAsync(int id, bool isAccepted, int modifiedByUserId);
        Task<bool> DeleteAsync(int id, int modifiedByUserId);
    }

    public class ExcuseService : IExcuseService
    {
        private readonly EduPlusDbContext _context;

        public ExcuseService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<ExcuseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false)
        {
            var query = from e in _context.Excuses.AsNoTracking()
                        join a in _context.Attendances on e.AttendanceId equals a.Id
                        join l in _context.Lessons on a.LessonId equals l.Id
                        join p in _context.Users on e.ParentId equals p.Id
                        join m in _context.Users on e.ModifiedByUserId equals m.Id into modifiedByJoin
                        from m in modifiedByJoin.DefaultIfEmpty()
                        where showInactive || e.IsActive
                        select new { e, a, l, p, m };

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                query = query.Where(x => x.p.LastName.ToLower().Contains(s) || x.e.Reason.ToLower().Contains(s));
            }

            var projected = query.Select(x => new ExcuseDto
            {
                Id = x.e.Id,
                ParentName = x.p.LastName + " " + x.p.FirstName,
                LessonDate = x.l.Date,
                IsAccepted = x.e.IsAccepted,
                AcceptedAt = x.e.AcceptedAt,
                ModifiedByName = x.m != null ? x.m.LastName + " " + x.m.FirstName : null,
                Reason = x.e.Reason,
                CreatedAt = x.e.CreatedAt
            });

            projected = sortBy?.ToLower() switch
            {
                "parent" => sortDesc ? projected.OrderByDescending(e => e.ParentName) : projected.OrderBy(e => e.ParentName),
                "lessondate" => sortDesc ? projected.OrderByDescending(e => e.LessonDate) : projected.OrderBy(e => e.LessonDate),
                "isaccepted" => sortDesc ? projected.OrderByDescending(e => e.IsAccepted) : projected.OrderBy(e => e.IsAccepted),
                "acceptedat" => sortDesc ? projected.OrderByDescending(e => e.AcceptedAt) : projected.OrderBy(e => e.AcceptedAt),
                "modifiedby" => sortDesc ? projected.OrderByDescending(e => e.ModifiedByName) : projected.OrderBy(e => e.ModifiedByName),
                _ => sortDesc ? projected.OrderByDescending(e => e.CreatedAt) : projected.OrderBy(e => e.CreatedAt)
            };

            var totalCount = await projected.CountAsync();
            var data = await projected.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResponse<ExcuseDto>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ExcuseDto?> GetByIdAsync(int id)
        {
            return await (from e in _context.Excuses.AsNoTracking()
                          join a in _context.Attendances on e.AttendanceId equals a.Id
                          join l in _context.Lessons on a.LessonId equals l.Id
                          join p in _context.Users on e.ParentId equals p.Id
                          join m in _context.Users on e.ModifiedByUserId equals m.Id into modifiedByJoin
                          from m in modifiedByJoin.DefaultIfEmpty()
                          where e.Id == id && e.IsActive
                          select new ExcuseDto
                          {
                              Id = e.Id,
                              ParentName = p.LastName + " " + p.FirstName,
                              LessonDate = l.Date,
                              IsAccepted = e.IsAccepted,
                              AcceptedAt = e.AcceptedAt,
                              ModifiedByName = m != null ? m.LastName + " " + m.FirstName : null,
                              Reason = e.Reason,
                              CreatedAt = e.CreatedAt
                          }).FirstOrDefaultAsync();
        }

        public async Task<Excuse> CreateAsync(CreateExcuseDto dto, int parentId)
        {
            var entity = new Excuse
            {
                AttendanceId = dto.AttendanceId,
                ParentId = parentId,
                Reason = dto.Reason,
                IsAccepted = null,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Excuses.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AcceptAsync(int id, bool isAccepted, int modifiedByUserId)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsAccepted = isAccepted;
            item.AcceptedAt = DateTime.Now;
            item.ModifiedByUserId = modifiedByUserId;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int modifiedByUserId)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsActive = false;
            item.ModifiedByUserId = modifiedByUserId;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
