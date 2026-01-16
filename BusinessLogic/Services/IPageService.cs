using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.CMS;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface IPageService
    {
        Task<IEnumerable<object>> GetAllAsync(int? targetId);
    }

    public class PageService : IPageService
    {
        private readonly EduPlusDbContext _context;

        public PageService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync(int? targetId)
        {
            var query = _context.Pages.AsNoTracking().AsQueryable();

            if (targetId.HasValue)
                query = query.Where(x => x.TargetId == targetId.Value);

            return await query
                .OrderBy(x => x.Title)
                .Select(x => new
                {
                    x.Id,
                    x.TargetId,
                    x.Link,
                    x.Title,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
