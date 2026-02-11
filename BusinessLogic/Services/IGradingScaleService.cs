using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IGradingScaleService
    {
        Task<IEnumerable<object>> GetAllAsync();
        Task<object?> UpdateAsync(int id, GradingScaleDto dto);
    }

    public class GradingScaleDto
    {
        public int GradeTypeId { get; set; }
        public decimal MinAverage { get; set; }
        public decimal MaxAverage { get; set; }
    }

    public class GradingScaleService : IGradingScaleService
    {
        private readonly EduPlusDbContext _context;

        public GradingScaleService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.GradingScales.AsNoTracking()
                .OrderBy(gs => gs.MinAverage)
                .Select(gs => new
                {
                    gs.Id,
                    gs.GradeTypeId,
                    GradeTypeName = gs.GradeType!.Numeric + " (" + gs.GradeType.Name + ")",
                    gs.MinAverage,
                    gs.MaxAverage,
                    gs.UpdatedAt,
                    ModifiedByName = _context.Users.Where(u => u.Id == gs.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault() ?? "System"
                })
                .ToListAsync();
        }

        public async Task<object?> UpdateAsync(int id, GradingScaleDto dto)
        {
            var existing = await _context.GradingScales
                .Include(gs => gs.GradeType)
                .FirstOrDefaultAsync(gs => gs.Id == id);
            if (existing == null) return null;

            var overlap = await _context.GradingScales
                .AnyAsync(gs => gs.Id != id && gs.MinAverage <= dto.MaxAverage && gs.MaxAverage >= dto.MinAverage);
            if (overlap) throw new InvalidOperationException("Zakres średnich nakłada się z inną oceną.");

            existing.MinAverage = dto.MinAverage;
            existing.MaxAverage = dto.MaxAverage;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return new
            {
                existing.Id,
                existing.GradeTypeId,
                GradeTypeName = existing.GradeType!.Numeric + " (" + existing.GradeType.Name + ")",
                existing.MinAverage,
                existing.MaxAverage,
                existing.UpdatedAt
            };
        }
    }
}
