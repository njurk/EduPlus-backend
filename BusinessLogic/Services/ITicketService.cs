using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface ITicketService
    {
        Task<PaginatedResponse<TicketDto>> GetAllAsync(int pageNumber, int pageSize, bool? showClosed, string? search, string? sortBy, bool sortDesc, int? reasonId = null, int? userId = null);
        Task<List<string>> GetSubmittersAsync();
        Task<TicketDto?> GetByIdAsync(int id);
        Task<Ticket> CreateAsync(CreateTicketDto dto, IEmailService emailService);
        Task<bool> CloseAsync(int id, CloseTicketDto dto, IEmailService emailService);
    }

    public class TicketService : ITicketService
    {
        private readonly EduPlusDbContext _context;

        public TicketService(EduPlusDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<TicketDto>> GetAllAsync(int pageNumber, int pageSize, bool? showClosed, string? search, string? sortBy, bool sortDesc, int? reasonId = null, int? userId = null)
        {
            var query = _context.Tickets.AsNoTracking().Include(t => t.Reason).AsQueryable();

            if (showClosed.HasValue)
                query = query.Where(t => t.IsClosed == showClosed.Value);

            if (reasonId.HasValue)
                query = query.Where(t => t.ReasonId == reasonId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(t =>
                    t.Email.ToLower().Contains(s) ||
                    t.Content.ToLower().Contains(s) ||
                    (t.Reason != null && t.Reason.Name.ToLower().Contains(s)));
            }

            query = sortBy?.ToLower() switch
            {
                "id" => sortDesc ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
                "email" => sortDesc ? query.OrderByDescending(t => t.Email) : query.OrderBy(t => t.Email),
                "reason" => sortDesc ? query.OrderByDescending(t => t.Reason!.Name) : query.OrderBy(t => t.Reason!.Name),
                "isclosed" or "closed" => sortDesc ? query.OrderByDescending(t => t.IsClosed) : query.OrderBy(t => t.IsClosed),
                "closedat" => sortDesc ? query.OrderByDescending(t => t.ClosedAt) : query.OrderBy(t => t.ClosedAt),
                "updated" or "updatedat" => sortDesc ? query.OrderByDescending(t => t.UpdatedAt) : query.OrderBy(t => t.UpdatedAt),
                "created" or "createdat" or _ => sortDesc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
            };

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Email = t.Email,
                    ReasonId = t.ReasonId,
                    ReasonName = t.Reason != null ? t.Reason.Name : "",
                    Content = t.Content,
                    IsClosed = t.IsClosed,
                    ClosedAt = t.ClosedAt,
                    AdminResponse = t.AdminResponse,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    ModifiedByName = t.ModifiedByUserId != null
                        ? _context.Users.Where(u => u.Id == t.ModifiedByUserId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault()
                        : "System",
                    IsRead = userId.HasValue ? _context.TicketReads.Any(tr => tr.TicketId == t.Id && tr.UserId == userId.Value) : null
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

        public async Task<List<string>> GetSubmittersAsync()
        {
            return await _context.Tickets
                .AsNoTracking()
                .Select(t => t.Email)
                .Distinct()
                .OrderBy(email => email)
                .ToListAsync();
        }

        public async Task<TicketDto?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.AsNoTracking()
                .Include(t => t.Reason)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return null;

            return new TicketDto
            {
                Id = ticket.Id,
                Email = ticket.Email,
                ReasonId = ticket.ReasonId,
                ReasonName = ticket.Reason?.Name ?? "",
                Content = ticket.Content,
                IsClosed = ticket.IsClosed,
                ClosedAt = ticket.ClosedAt,
                AdminResponse = ticket.AdminResponse,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }

        public async Task<Ticket> CreateAsync(CreateTicketDto dto, IEmailService emailService)
        {
            if (!await _context.TicketReasons.AnyAsync(tr => tr.Id == dto.ReasonId && tr.IsActive))
                throw new InvalidOperationException("Wybrany powód zgłoszenia nie istnieje lub jest nieaktywny");

            var ticket = new Ticket
            {
                Email = dto.Email,
                ReasonId = dto.ReasonId,
                Content = dto.Content,
                IsClosed = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            try
            {
                var reason = await _context.TicketReasons.FindAsync(dto.ReasonId);
                await emailService.SendTicketCreatedEmailAsync(dto.Email, ticket.Id, reason?.Name ?? "Zgłoszenie", dto.Content);
            }
            catch 
            { 
            }

            return ticket;
        }

        public async Task<bool> CloseAsync(int id, CloseTicketDto dto, IEmailService emailService)
        {
            var ticket = await _context.Tickets.Include(t => t.Reason).FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null || ticket.IsClosed) return false;

            ticket.IsClosed = true;
            ticket.ClosedAt = DateTime.Now;
            ticket.AdminResponse = dto.AdminResponse;
            ticket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            var modifiedBy = "System";
            if (ticket.ModifiedByUserId.HasValue)
            {
                var user = await _context.Users.FindAsync(ticket.ModifiedByUserId.Value);
                if (user != null) modifiedBy = $"{user.FirstName} {user.LastName}";
            }

            await emailService.SendTicketClosedEmailAsync(ticket.Email, ticket.Id, ticket.Reason?.Name ?? "Zgłoszenie", ticket.Content, dto.AdminResponse, modifiedBy);

            return true;
        }
    }
}

