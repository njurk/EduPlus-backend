namespace Shared.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int ReasonId { get; set; }
        public string ReasonName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
