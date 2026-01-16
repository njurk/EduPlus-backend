using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAnnouncementReadService
    {
        Task<IEnumerable<AnnouncementRead>> GetAllAsync();
        Task<AnnouncementRead> CreateAsync(AnnouncementRead entity);
        Task<bool> DeleteAsync(int id);
    }

    public class AnnouncementReadService : IAnnouncementReadService
    {
        private readonly EduPlusDbContext _context;

        public AnnouncementReadService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnnouncementRead>> GetAllAsync()
        {
            return await _context.AnnouncementReads
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AnnouncementRead> CreateAsync(AnnouncementRead entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            _context.AnnouncementReads.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.AnnouncementReads.FindAsync(id);
            if (item == null) return false;

            _context.AnnouncementReads.Remove(item);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
