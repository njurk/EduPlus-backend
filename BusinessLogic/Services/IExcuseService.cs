using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IExcuseService
    {
        Task<PaginatedResponse<ExcuseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? statusFilter = null, int? classId = null);
        Task<ExcuseDetailsDto?> GetByIdAsync(int id);
        Task<Excuse> CreateAsync(CreateExcuseDto dto, int parentId);
        Task<bool> AcceptAsync(int id, bool? isAccepted, int modifiedByUserId);
        Task<bool> RestoreAsync(int id, int modifiedByUserId);
        Task<bool> DeleteAsync(int id, int modifiedByUserId);
    }

    public class ExcuseService : IExcuseService
    {
        private readonly EduPlusDbContext _context;

        public ExcuseService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<ExcuseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20, string? search = null, string? sortBy = null, bool sortDesc = true, bool showInactive = false, string? statusFilter = null, int? classId = null)
        {
            var query = from e in _context.Excuses.AsNoTracking()
                        join s in _context.Users on e.StudentId equals s.Id
                        join p in _context.Users on e.ParentId equals p.Id
                        join m in _context.Users on e.ModifiedByUserId equals m.Id into modifiedByJoin
                        from m in modifiedByJoin.DefaultIfEmpty()
                        join sc in _context.ClassStudents on e.StudentId equals sc.StudentId into scJoin
                        from sc in scJoin.DefaultIfEmpty()
                        join c in _context.Classes on sc.ClassId equals c.Id into classJoin
                        from c in classJoin.DefaultIfEmpty()
                        where e.IsActive == !showInactive
                        select new
                        {
                            e,
                            s,
                            p,
                            m,
                            ClassId = c != null ? (int?)c.Id : null,
                            ClassName = c != null ? c.Level + c.Letter : null,
                            Attendances = _context.ExcuseAttendances
                                .Where(ea => ea.ExcuseId == e.Id)
                                .Join(_context.Attendances, ea => ea.AttendanceId, a => a.Id, (ea, a) => a)
                                .Join(_context.Lessons, a => a.LessonId, l => l.Id, (a, l) => l.Date)
                                .ToList()
                        };

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(x => x.p.LastName.ToLower().Contains(searchLower) 
                    || x.s.LastName.ToLower().Contains(searchLower)
                    || x.e.Reason.ToLower().Contains(searchLower)
                    || (x.ClassName != null && x.ClassName.ToLower().Contains(searchLower)));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = statusFilter switch
                {
                    "pending" => query.Where(x => x.e.IsAccepted == null),
                    "accepted" => query.Where(x => x.e.IsAccepted == true),
                    "rejected" => query.Where(x => x.e.IsAccepted == false),
                    _ => query
                };
            }

            if (classId.HasValue)
            {
                query = query.Where(x => x.ClassId == classId.Value);
            }

            var projected = query.Select(x => new ExcuseDto
            {
                Id = x.e.Id,
                ParentName = x.p.LastName + " " + x.p.FirstName,
                StudentName = x.s.LastName + " " + x.s.FirstName,
                ClassName = x.ClassName,
                IsAccepted = x.e.IsAccepted,
                AcceptedAt = x.e.AcceptedAt,
                ModifiedByName = x.m != null ? x.m.LastName + " " + x.m.FirstName : null,
                Reason = x.e.Reason,
                CreatedAt = x.e.CreatedAt,
                AttendanceCount = x.Attendances.Count
            });

            projected = sortBy?.ToLower() switch
            {
                "parent" => sortDesc ? projected.OrderByDescending(e => e.ParentName) : projected.OrderBy(e => e.ParentName),
                "student" => sortDesc ? projected.OrderByDescending(e => e.StudentName) : projected.OrderBy(e => e.StudentName),
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

        public async Task<ExcuseDetailsDto?> GetByIdAsync(int id)
        {
            var excuse = await _context.Excuses.AsNoTracking()
                .Where(e => e.Id == id && e.IsActive)
                .FirstOrDefaultAsync();

            if (excuse == null) return null;

            var student = await _context.Users.FindAsync(excuse.StudentId);
            var parent = await _context.Users.FindAsync(excuse.ParentId);
            var modifiedBy = excuse.ModifiedByUserId.HasValue 
                ? await _context.Users.FindAsync(excuse.ModifiedByUserId.Value) 
                : null;

            var attendances = await (from ea in _context.ExcuseAttendances
                                     join a in _context.Attendances on ea.AttendanceId equals a.Id
                                     join l in _context.Lessons on a.LessonId equals l.Id
                                     join s in _context.Subjects on l.SubjectId equals s.Id
                                     join lh in _context.LessonHours on l.LessonHourId equals lh.Id
                                     where ea.ExcuseId == id
                                     orderby l.Date, lh.OrderNumber
                                     select new ExcuseAttendanceItemDto
                                     {
                                         Id = a.Id,
                                         Date = l.Date,
                                         SubjectName = s.Name,
                                         LessonHour = lh.OrderNumber
                                     }).ToListAsync();

            return new ExcuseDetailsDto
            {
                Id = excuse.Id,
                ParentName = parent != null ? $"{parent.LastName} {parent.FirstName}" : "",
                StudentName = student != null ? $"{student.LastName} {student.FirstName}" : "",
                IsAccepted = excuse.IsAccepted,
                AcceptedAt = excuse.AcceptedAt,
                ModifiedByName = modifiedBy != null ? $"{modifiedBy.LastName} {modifiedBy.FirstName}" : null,
                Reason = excuse.Reason,
                CreatedAt = excuse.CreatedAt,
                Attendances = attendances
            };
        }

        public async Task<Excuse> CreateAsync(CreateExcuseDto dto, int parentId)
        {
            var entity = new Excuse
            {
                StudentId = dto.StudentId,
                ParentId = parentId,
                Reason = dto.Reason,
                IsAccepted = null,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Excuses.Add(entity);
            await _context.SaveChangesAsync();

            foreach (var attendanceId in dto.AttendanceIds)
            {
                _context.ExcuseAttendances.Add(new ExcuseAttendance
                {
                    ExcuseId = entity.Id,
                    AttendanceId = attendanceId
                });
            }
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> AcceptAsync(int id, bool? isAccepted, int modifiedByUserId)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsAccepted = isAccepted;
            item.AcceptedAt = isAccepted.HasValue ? DateTime.Now : null;
            item.ModifiedByUserId = modifiedByUserId;
            item.UpdatedAt = DateTime.Now;

            if (isAccepted == true)
            {
                var attendanceIds = await _context.ExcuseAttendances
                    .Where(ea => ea.ExcuseId == id)
                    .Select(ea => ea.AttendanceId)
                    .ToListAsync();

                var attendances = await _context.Attendances
                    .Where(a => attendanceIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var attendance in attendances)
                {
                    attendance.AttendanceTypeId = 4;
                    attendance.UpdatedAt = DateTime.Now;
                    attendance.ModifiedByUserId = modifiedByUserId;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int modifiedByUserId)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            if (!item.IsActive)
            {
                var excuseAttendances = _context.ExcuseAttendances.Where(ea => ea.ExcuseId == id);
                _context.ExcuseAttendances.RemoveRange(excuseAttendances);
                _context.Excuses.Remove(item);
            }
            else
            {
                item.IsActive = false;
                item.ModifiedByUserId = modifiedByUserId;
                item.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id, int modifiedByUserId)
        {
            var item = await _context.Excuses.FindAsync(id);
            if (item == null) return false;

            item.IsActive = true;
            item.ModifiedByUserId = modifiedByUserId;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
