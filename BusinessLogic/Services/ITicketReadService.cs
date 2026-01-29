using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public interface ITicketReadService
    {
        Task<IEnumerable<TicketRead>> GetAllAsync();
        Task<TicketRead> CreateAsync(TicketRead entity);
        Task<bool> DeleteAsync(int id);
        Task MarkAsReadAsync(int ticketId, int userId);
    }

    public class TicketReadService : ITicketReadService
    {
        private readonly EduPlusDbContext _context;

        public TicketReadService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketRead>> GetAllAsync()
        {
            return await _context.TicketReads
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TicketRead> CreateAsync(TicketRead entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            _context.TicketReads.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.TicketReads.FindAsync(id);
            if (item == null) return false;

            _context.TicketReads.Remove(item);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task MarkAsReadAsync(int ticketId, int userId)
        {
            var exists = await _context.TicketReads
                .AnyAsync(tr => tr.TicketId == ticketId && tr.UserId == userId);
            if (exists) return;

            _context.TicketReads.Add(new TicketRead
            {
                TicketId = ticketId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
    }
}
