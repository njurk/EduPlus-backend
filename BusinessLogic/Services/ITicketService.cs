using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface ITicketService
    {
        Task<PaginatedResponse<TicketDto>> GetAllAsync(int pageNumber, int pageSize, bool? showClosed, string? search, string? sortBy, bool sortDesc);
        Task<TicketDto?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(int userId, CreateTicketDto dto, IEmailService emailService);
        Task<bool> CloseAsync(int id, CloseTicketDto dto, IEmailService emailService);
    }

    public class TicketService : ITicketService
    {
        private readonly EduPlusDbContext _context;

        public TicketService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<TicketDto>> GetAllAsync(int pageNumber, int pageSize, bool? showClosed, string? search, string? sortBy, bool sortDesc)
        {
            var query = _context.Tickets.AsNoTracking().Include(t => t.User).AsQueryable();

            if (showClosed.HasValue)
                query = query.Where(t => t.IsClosed == showClosed.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(t =>
                    t.Subject.ToLower().Contains(s) ||
                    t.User.FirstName.ToLower().Contains(s) ||
                    t.User.LastName.ToLower().Contains(s) ||
                    t.User.Email.ToLower().Contains(s));
            }

            query = sortBy?.ToLower() switch
            {
                "subject" => sortDesc ? query.OrderByDescending(t => t.Subject) : query.OrderBy(t => t.Subject),
                "userfullname" => sortDesc
                    ? query.OrderByDescending(t => t.User.LastName).ThenByDescending(t => t.User.FirstName)
                    : query.OrderBy(t => t.User.LastName).ThenBy(t => t.User.FirstName),
                "isclosed" => sortDesc ? query.OrderByDescending(t => t.IsClosed) : query.OrderBy(t => t.IsClosed),
                _ => sortDesc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
            };

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    UserFullName = t.User.LastName + " " + t.User.FirstName,
                    UserEmail = t.User.Email,
                    Subject = t.Subject,
                    IsClosed = t.IsClosed,
                    ClosedAt = t.ClosedAt,
                    AdminResponse = t.AdminResponse,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                })
                .ToListAsync();

            return new PaginatedResponse<TicketDto>
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = data
            };
        }

        public async Task<TicketDto?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.AsNoTracking()
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return null;

            return new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                UserFullName = ticket.User.LastName + " " + ticket.User.FirstName,
                UserEmail = ticket.User.Email,
                Subject = ticket.Subject,
                IsClosed = ticket.IsClosed,
                ClosedAt = ticket.ClosedAt,
                AdminResponse = ticket.AdminResponse,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }

        public async Task<Ticket> CreateAsync(int userId, CreateTicketDto dto, IEmailService emailService)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("Użytkownik nie istnieje");

            var ticket = new Ticket
            {
                UserId = userId,
                Subject = dto.Subject,
                IsClosed = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            try
            {
                await emailService.SendTicketCreatedEmailAsync(user.Email, ticket.Id, ticket.Subject);
            }
            catch 
            { 
                
            }

            return ticket;
        }

        public async Task<bool> CloseAsync(int id, CloseTicketDto dto, IEmailService emailService)
        {
            var ticket = await _context.Tickets.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null || ticket.IsClosed) return false;

            ticket.IsClosed = true;
            ticket.ClosedAt = DateTime.Now;
            ticket.AdminResponse = dto.AdminResponse;
            ticket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            await emailService.SendTicketClosedEmailAsync(ticket.User.Email, ticket.Id, ticket.Subject, dto.AdminResponse);

            return true;
        }
    }
}
