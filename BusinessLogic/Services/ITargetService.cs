using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Data.CMS;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ITargetService
    {
        Task<IEnumerable<object>> GetAllAsync();
    }

    public class TargetService : ITargetService
    {
        private readonly EduPlusDbContext _context;

        public TargetService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await _context.Targets.AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Label,
                    x.Title,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
